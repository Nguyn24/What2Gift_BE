import pandas as pd
import psycopg2
import uuid
import sys
from psycopg2.extras import execute_values

# Connection string from appsettings.Production.json
CONNECTION_STRING = "host=dpg-d3frdk2dbo4c73e5qgag-a.oregon-postgres.render.com port=5432 dbname=what2gift_db user=what2gift_db_user password=6sIV4jAML7nQqYF1gHqmZkHp80VwCZGE"

# Mapping brands
brand_mapping = {
    'Canifa': '550e8400-e29b-41d4-a716-446655440001',
    'IVY Moda': '550e8400-e29b-41d4-a716-446655440002',
    'Levi\'s': '550e8400-e29b-41d4-a716-446655440003',
    'H&M': '550e8400-e29b-41d4-a716-446655440004',
    'Routine': '550e8400-e29b-41d4-a716-446655440005',
    'NEM': '550e8400-e29b-41d4-a716-446655440006',
    'Lacoste': '550e8400-e29b-41d4-a716-446655440007',
    'Uniqlo': '550e8400-e29b-41d4-a716-446655440008',
    'Nike': '550e8400-e29b-41d4-a716-446655440009',
    'Zara': '550e8400-e29b-41d4-a716-446655440010',
    'Local': '550e8400-e29b-41d4-a716-446655440011',
    'Casio': '550e8400-e29b-41d4-a716-446655440012',
    'Daniel Wellington': '550e8400-e29b-41d4-a716-446655440013',
    'Charles & Keith': '550e8400-e29b-41d4-a716-446655440014',
    'Adidas': '550e8400-e29b-41d4-a716-446655440015',
    'PNJ': '550e8400-e29b-41d4-a716-446655440016',
    'Ray-Ban': '550e8400-e29b-41d4-a716-446655440017',
    'Gucci': '550e8400-e29b-41d4-a716-446655440018',
    'Apple': '550e8400-e29b-41d4-a716-446655440019',
    'Samsonite': '550e8400-e29b-41d4-a716-446655440020',
    'Gentle Monster': '550e8400-e29b-41d4-a716-446655440021',
    'Citizen': '550e8400-e29b-41d4-a716-446655440022',
    'The North Face': '550e8400-e29b-41d4-a716-446655440023',
    'Orient': '550e8400-e29b-41d4-a716-446655440024',
    'Prada': '550e8400-e29b-41d4-a716-446655440025',
    'Xiaomi': '550e8400-e29b-41d4-a716-446655440026',
    'Seiko': '550e8400-e29b-41d4-a716-446655440027',
    'Dior': '550e8400-e29b-41d4-a716-446655440028',
    'CK': '550e8400-e29b-41d4-a716-446655440029',
    'G-Shock': '550e8400-e29b-41d4-a716-446655440030',
    'DW': '550e8400-e29b-41d4-a716-446655440031',
    'Chanel': '550e8400-e29b-41d4-a716-446655440032',
    'YSL': '550e8400-e29b-41d4-a716-446655440033',
    'Samsung': '550e8400-e29b-41d4-a716-446655440034',
    'Vans': '550e8400-e29b-41d4-a716-446655440035',
    'Burberry': '550e8400-e29b-41d4-a716-446655440036',
    'Oakley': '550e8400-e29b-41d4-a716-446655440037'
}

# Mapping categories
category_mapping = {
    'shirt': '660e8400-e29b-41d4-a716-446655440001',
    'pants': '660e8400-e29b-41d4-a716-446655440002',
    'dress': '660e8400-e29b-41d4-a716-446655440003',
    'jacket': '660e8400-e29b-41d4-a716-446655440004',
    'hoodie': '660e8400-e29b-41d4-a716-446655440005',
    'shorts': '660e8400-e29b-41d4-a716-446655440006',
    'sweater': '660e8400-e29b-41d4-a716-446655440007',
    'leggings': '660e8400-e29b-41d4-a716-446655440008',
    'vest': '660e8400-e29b-41d4-a716-446655440009',
    'watch': '660e8400-e29b-41d4-a716-446655440010',
    'wallet': '660e8400-e29b-41d4-a716-446655440011',
    'bag': '660e8400-e29b-41d4-a716-446655440012',
    'shoes': '660e8400-e29b-41d4-a716-446655440013',
    'hat': '660e8400-e29b-41d4-a716-446655440014',
    'accessory': '660e8400-e29b-41d4-a716-446655440015',
    'belt': '660e8400-e29b-41d4-a716-446655440016',
    'scarf': '660e8400-e29b-41d4-a716-446655440017',
    'socks': '660e8400-e29b-41d4-a716-446655440018',
    'glasses': '660e8400-e29b-41d4-a716-446655440019',
    'perfume': '660e8400-e29b-41d4-a716-446655440020'
}

def connect_to_database():
    """Connect to PostgreSQL database on Render"""
    try:
        conn = psycopg2.connect(CONNECTION_STRING)
        print("Successfully connected to Render database!")
        return conn
    except Exception as e:
        print(f"Database connection error: {e}")
        return None

def clear_existing_data(cursor):
    """Clear existing data (if any)"""
    try:
        print("Clearing existing data...")
        cursor.execute('DELETE FROM "ProductSources"')
        cursor.execute('DELETE FROM "Products"')
        cursor.execute('DELETE FROM "Brands"')
        cursor.execute('DELETE FROM "Categories"')
        print("Existing data cleared")
    except Exception as e:
        print(f"Note: {e}")

def insert_brands(cursor):
    """Insert brands"""
    print("Inserting brands...")
    
    brands_data = [
        ('550e8400-e29b-41d4-a716-446655440001', 'Canifa', 'Vietnamese fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440002', 'IVY Moda', 'Premium women fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440003', 'Levi\'s', 'World famous jeans brand'),
        ('550e8400-e29b-41d4-a716-446655440004', 'H&M', 'Global fast fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440005', 'Routine', 'Premium men fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440006', 'NEM', 'Vietnamese fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440007', 'Lacoste', 'Premium French fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440008', 'Uniqlo', 'Japanese fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440009', 'Nike', 'World leading sports brand'),
        ('550e8400-e29b-41d4-a716-446655440010', 'Zara', 'Spanish fast fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440011', 'Local', 'Local fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440012', 'Casio', 'Japanese watch brand'),
        ('550e8400-e29b-41d4-a716-446655440013', 'Daniel Wellington', 'Swedish watch brand'),
        ('550e8400-e29b-41d4-a716-446655440014', 'Charles & Keith', 'Singapore bag brand'),
        ('550e8400-e29b-41d4-a716-446655440015', 'Adidas', 'German sports brand'),
        ('550e8400-e29b-41d4-a716-446655440016', 'PNJ', 'Vietnamese jewelry brand'),
        ('550e8400-e29b-41d4-a716-446655440017', 'Ray-Ban', 'American sunglasses brand'),
        ('550e8400-e29b-41d4-a716-446655440018', 'Gucci', 'Premium Italian fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440019', 'Apple', 'American technology brand'),
        ('550e8400-e29b-41d4-a716-446655440020', 'Samsonite', 'Luggage brand'),
        ('550e8400-e29b-41d4-a716-446655440021', 'Gentle Monster', 'Korean sunglasses brand'),
        ('550e8400-e29b-41d4-a716-446655440022', 'Citizen', 'Japanese watch brand'),
        ('550e8400-e29b-41d4-a716-446655440023', 'The North Face', 'American outdoor brand'),
        ('550e8400-e29b-41d4-a716-446655440024', 'Orient', 'Japanese watch brand'),
        ('550e8400-e29b-41d4-a716-446655440025', 'Prada', 'Premium Italian fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440026', 'Xiaomi', 'Chinese technology brand'),
        ('550e8400-e29b-41d4-a716-446655440027', 'Seiko', 'Japanese watch brand'),
        ('550e8400-e29b-41d4-a716-446655440028', 'Dior', 'Premium French fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440029', 'CK', 'Calvin Klein - American fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440030', 'G-Shock', 'Casio sports watch brand'),
        ('550e8400-e29b-41d4-a716-446655440031', 'DW', 'Daniel Wellington'),
        ('550e8400-e29b-41d4-a716-446655440032', 'Chanel', 'Premium French fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440033', 'YSL', 'Yves Saint Laurent - French fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440034', 'Samsung', 'Korean technology brand'),
        ('550e8400-e29b-41d4-a716-446655440035', 'Vans', 'American sneaker brand'),
        ('550e8400-e29b-41d4-a716-446655440036', 'Burberry', 'Premium British fashion brand'),
        ('550e8400-e29b-41d4-a716-446655440037', 'Oakley', 'American sports sunglasses brand')
    ]
    
    insert_query = '''
        INSERT INTO "Brands" ("Id", "Name", "Description") 
        VALUES (%s, %s, %s)
    '''
    
    cursor.executemany(insert_query, brands_data)
    print(f"Inserted {len(brands_data)} brands")

def insert_categories(cursor):
    """Insert categories"""
    print("Inserting categories...")
    
    categories_data = [
        ('660e8400-e29b-41d4-a716-446655440001', 'shirt', 'Shirts, t-shirts, polo shirts'),
        ('660e8400-e29b-41d4-a716-446655440002', 'pants', 'Long pants, jeans, dress pants'),
        ('660e8400-e29b-41d4-a716-446655440003', 'dress', 'Dresses of all types'),
        ('660e8400-e29b-41d4-a716-446655440004', 'jacket', 'Jackets, blazers, vests'),
        ('660e8400-e29b-41d4-a716-446655440005', 'hoodie', 'Hoodies, pullovers with hood'),
        ('660e8400-e29b-41d4-a716-446655440006', 'shorts', 'Shorts, briefs'),
        ('660e8400-e29b-41d4-a716-446655440007', 'sweater', 'Sweaters, pullovers'),
        ('660e8400-e29b-41d4-a716-446655440008', 'leggings', 'Leggings, tight pants'),
        ('660e8400-e29b-41d4-a716-446655440009', 'vest', 'Vests, waistcoats'),
        ('660e8400-e29b-41d4-a716-446655440010', 'watch', 'Wrist watches'),
        ('660e8400-e29b-41d4-a716-446655440011', 'wallet', 'Wallets, handbags'),
        ('660e8400-e29b-41d4-a716-446655440012', 'bag', 'Handbags, backpacks, briefcases'),
        ('660e8400-e29b-41d4-a716-446655440013', 'shoes', 'Shoes of all types'),
        ('660e8400-e29b-41d4-a716-446655440014', 'hat', 'Hats, caps of all types'),
        ('660e8400-e29b-41d4-a716-446655440015', 'accessory', 'Jewelry accessories'),
        ('660e8400-e29b-41d4-a716-446655440016', 'belt', 'Belts, waistbands'),
        ('660e8400-e29b-41d4-a716-446655440017', 'scarf', 'Scarves, neck wraps'),
        ('660e8400-e29b-41d4-a716-446655440018', 'socks', 'Socks, stockings'),
        ('660e8400-e29b-41d4-a716-446655440019', 'glasses', 'Sunglasses, prescription glasses'),
        ('660e8400-e29b-41d4-a716-446655440020', 'perfume', 'Perfumes, fragrances')
    ]
    
    insert_query = '''
        INSERT INTO "Categories" ("Id", "Name", "Description") 
        VALUES (%s, %s, %s)
    '''
    
    cursor.executemany(insert_query, categories_data)
    print(f"Inserted {len(categories_data)} categories")

def insert_products(cursor, df):
    """Insert products"""
    print("Inserting products...")
    
    products_data = []
    for index, row in df.iterrows():
        product_id = str(uuid.uuid4())
        brand_id = brand_mapping.get(row['brand'], '550e8400-e29b-41d4-a716-446655440011')  # Default to Local
        category_id = category_mapping.get(row['category'], '660e8400-e29b-41d4-a716-446655440001')  # Default to shirt
        name = row['product_name']
        description = row['description'] if pd.notna(row['description']) else None
        image_url = row['image_url'] if pd.notna(row['image_url']) else None
        
        products_data.append((product_id, brand_id, category_id, name, description, image_url))
    
    insert_query = '''
        INSERT INTO "Products" ("Id", "BrandId", "CategoryId", "OccasionId", "Name", "Description", "ImageUrl") 
        VALUES (%s, %s, %s, NULL, %s, %s, %s)
    '''
    
    cursor.executemany(insert_query, products_data)
    print(f"Inserted {len(products_data)} products")
    
    return products_data

def insert_product_sources(cursor, products_data, df):
    """Insert product sources"""
    print("Inserting product sources...")
    
    sources_data = []
    for i, (product_id, _, _, _, _, _) in enumerate(products_data):
        source_id = str(uuid.uuid4())
        vendor_name = "Default Vendor"
        price = int(df.iloc[i]['price'])  # Convert numpy.int64 to Python int
        
        sources_data.append((source_id, product_id, vendor_name, price))
    
    insert_query = '''
        INSERT INTO "ProductSources" ("Id", "ProductId", "VendorName", "Price", "AffiliateLink") 
        VALUES (%s, %s, %s, %s, '')
    '''
    
    cursor.executemany(insert_query, sources_data)
    print(f"Inserted {len(sources_data)} product sources")

def verify_data(cursor):
    """Verify data after import"""
    print("\nVerifying data...")
    
    cursor.execute('SELECT COUNT(*) FROM "Brands"')
    brands_count = cursor.fetchone()[0]
    print(f"Brands: {brands_count}")
    
    cursor.execute('SELECT COUNT(*) FROM "Categories"')
    categories_count = cursor.fetchone()[0]
    print(f"Categories: {categories_count}")
    
    cursor.execute('SELECT COUNT(*) FROM "Products"')
    products_count = cursor.fetchone()[0]
    print(f"Products: {products_count}")
    
    cursor.execute('SELECT COUNT(*) FROM "ProductSources"')
    sources_count = cursor.fetchone()[0]
    print(f"Product Sources: {sources_count}")
    
    # Check foreign key relationships
    cursor.execute('''
        SELECT p."Name", b."Name" as Brand, c."Name" as Category 
        FROM "Products" p
        JOIN "Brands" b ON p."BrandId" = b."Id"
        JOIN "Categories" c ON p."CategoryId" = c."Id"
        LIMIT 5
    ''')
    
    print("\nSample products:")
    for row in cursor.fetchall():
        print(f"  - {row[0]} ({row[1]} - {row[2]})")

def main():
    """Main function"""
    print("Starting data import to PostgreSQL on Render...")
    
    # Read CSV
    try:
        df = pd.read_csv(r'C:\Users\ADMIN\Downloads\database.csv')
        print(f"Read CSV with {len(df)} products")
    except Exception as e:
        print(f"CSV read error: {e}")
        return
    
    # Connect to database
    conn = connect_to_database()
    if not conn:
        return
    
    try:
        cursor = conn.cursor()
        
        # Clear existing data
        clear_existing_data(cursor)
        
        # Insert data
        insert_brands(cursor)
        insert_categories(cursor)
        products_data = insert_products(cursor, df)
        insert_product_sources(cursor, products_data, df)
        
        # Commit transaction
        conn.commit()
        print("\nAll changes committed!")
        
        # Verify data
        verify_data(cursor)
        
        print("\nImport completed successfully!")
        print("Data has been imported to Render database")
        
    except Exception as e:
        print(f"Error during import: {e}")
        conn.rollback()
    finally:
        cursor.close()
        conn.close()
        print("Database connection closed")

if __name__ == "__main__":
    main()
