import csv
import psycopg2
import uuid
from urllib.parse import urlparse

# Database connection string from appsettings.Production.json
CONNECTION_STRING = "Host=dpg-d4883hfgi27c73cgigeg-a.singapore-postgres.render.com;Port=5432;Database=what2gift;Username=what2gift_user;Password=ApTABsMJfNkHx1cwUAY7QqWX4VizFwsp;Ssl Mode=Require;Trust Server Certificate=true;"

def parse_connection_string(conn_str):
    """Parse connection string to dictionary"""
    params = {}
    for part in conn_str.split(';'):
        if '=' in part:
            key, value = part.split('=', 1)
            key = key.strip()
            value = value.strip()
            if key == "Host":
                params['host'] = value
            elif key == "Port":
                params['port'] = value
            elif key == "Database":
                params['database'] = value
            elif key == "Username":
                params['user'] = value
            elif key == "Password":
                params['password'] = value
            elif key == "Ssl Mode":
                # psycopg2 uses sslmode parameter
                if value.lower() == "require":
                    params['sslmode'] = 'require'
                else:
                    params['sslmode'] = value.lower()
            elif key == "Trust Server Certificate":
                # Handle trust server certificate
                if value.lower() == "true":
                    params['sslmode'] = 'require'  # Use require mode when trusting server cert
    return params

def get_or_create_brand(cursor, brand_name):
    """Get existing brand or create new one"""
    # Check if brand exists
    cursor.execute('SELECT "Id" FROM "Brands" WHERE "Name" = %s', (brand_name,))
    result = cursor.fetchone()
    
    if result:
        return result[0]
    
    # Create new brand
    brand_id = uuid.uuid4()
    cursor.execute(
        'INSERT INTO "Brands" ("Id", "Name") VALUES (%s, %s)',
        (str(brand_id), brand_name)
    )
    return brand_id

def get_or_create_category(cursor, category_name):
    """Get existing category or create new one"""
    # Normalize category name (capitalize first letter)
    category_name = category_name.strip()
    if not category_name:
        raise ValueError("Category name cannot be empty")
    
    # Capitalize first letter, keep rest as-is
    if len(category_name) > 1:
        category_name = category_name[0].upper() + category_name[1:]
    else:
        category_name = category_name.upper()
    
    # Check if category exists (case-insensitive)
    cursor.execute('SELECT "Id" FROM "Categories" WHERE LOWER("Name") = LOWER(%s)', (category_name,))
    result = cursor.fetchone()
    
    if result:
        return result[0]
    
    # Create new category
    category_id = uuid.uuid4()
    cursor.execute(
        'INSERT INTO "Categories" ("Id", "Name") VALUES (%s, %s)',
        (str(category_id), category_name)
    )
    return category_id

def extract_vendor_name(product_link):
    """Extract vendor name from product link"""
    if not product_link or product_link.strip() == '':
        return "Unknown"
    
    try:
        parsed = urlparse(product_link)
        domain = parsed.netloc.lower()
        
        if 'shopee' in domain:
            return "Shopee"
        elif 'lazada' in domain:
            return "Lazada"
        elif 'tiki' in domain:
            return "Tiki"
        elif 'sendo' in domain:
            return "Sendo"
        else:
            # Extract domain name
            parts = domain.split('.')
            if len(parts) >= 2:
                return parts[-2].capitalize()
            return "Unknown"
    except:
        return "Unknown"

def import_products(csv_file_path):
    """Import products from CSV file"""
    # Parse connection string
    conn_params = parse_connection_string(CONNECTION_STRING)
    
    # Connect to database
    conn = psycopg2.connect(**conn_params)
    cursor = conn.cursor()
    
    try:
        # Read CSV file
        with open(csv_file_path, 'r', encoding='utf-8') as csvfile:
            reader = csv.DictReader(csvfile)
            
            imported_count = 0
            skipped_count = 0
            
            for row in reader:
                try:
                    # Get or create brand
                    brand_name = row['brand'].strip()
                    if not brand_name:
                        print(f"Skipping row {row.get('product_id', 'unknown')}: Missing brand")
                        skipped_count += 1
                        continue
                    
                    brand_id = get_or_create_brand(cursor, brand_name)
                    
                    # Get or create category
                    category_name = row['category'].strip()
                    if not category_name:
                        print(f"Skipping row {row.get('product_id', 'unknown')}: Missing category")
                        skipped_count += 1
                        continue
                    
                    category_id = get_or_create_category(cursor, category_name)
                    
                    # Create product
                    product_id = uuid.uuid4()
                    product_name = row['product_name'].strip()
                    if len(product_name) > 255:
                        product_name = product_name[:255]
                    
                    description = row.get('description', '').strip()
                    if description and len(description) > 1000:
                        description = description[:1000]
                    
                    image_url = row.get('image_url', '').strip()
                    if image_url and len(image_url) > 1000:
                        image_url = image_url[:1000]
                    
                    cursor.execute(
                        """INSERT INTO "Products" ("Id", "BrandId", "CategoryId", "OccasionId", "Name", "Description", "ImageUrl")
                           VALUES (%s, %s, %s, NULL, %s, %s, %s)""",
                        (str(product_id), str(brand_id), str(category_id), product_name, description or None, image_url or None)
                    )
                    
                    # Create product source
                    price = float(row.get('price', 0))
                    product_link = row.get('product_link', '').strip()
                    
                    if product_link:
                        vendor_name = extract_vendor_name(product_link)
                        if len(vendor_name) > 255:
                            vendor_name = vendor_name[:255]
                        
                        if len(product_link) > 1000:
                            product_link = product_link[:1000]
                        
                        product_source_id = uuid.uuid4()
                        cursor.execute(
                            """INSERT INTO "ProductSources" ("Id", "ProductId", "VendorName", "Price", "AffiliateLink")
                               VALUES (%s, %s, %s, %s, %s)""",
                            (str(product_source_id), str(product_id), vendor_name, price, product_link)
                        )
                    
                    imported_count += 1
                    
                    if imported_count % 50 == 0:
                        conn.commit()
                        print(f"Imported {imported_count} products...")
                
                except Exception as e:
                    print(f"Error importing row {row.get('product_id', 'unknown')}: {str(e)}")
                    conn.rollback()
                    skipped_count += 1
                    continue
            
            # Final commit
            conn.commit()
            
            print(f"\nImport completed!")
            print(f"Successfully imported: {imported_count} products")
            print(f"Skipped: {skipped_count} products")
    
    except Exception as e:
        conn.rollback()
        print(f"Error during import: {str(e)}")
        raise
    
    finally:
        cursor.close()
        conn.close()

if __name__ == "__main__":
    import_products("database.csv")

