using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using What2Gift.Domain.EmailTemplates;

namespace What2Gift.Infrastructure.Configuration;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Content).IsRequired().HasColumnType("text");
        builder.Property(e => e.Header).IsRequired().HasColumnType("text");
        builder.Property(e => e.MainContent).IsRequired().HasColumnType("text");

        // builder.HasData(
        //     new EmailTemplate
        //     {
        //         Id = 1,
        //         Header = "Verify Email",
        //         Content = @"
        //             <!DOCTYPE html>
        //             <html>
        //             <head>
        //                 <meta charset=""UTF-8"" />
        //                 <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
        //                 <title>Email Template</title>
        //                 <link href=""https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css"" rel=""stylesheet"" />
        //             </head>
        //             <body class=""bg-gray-100"">
        //                 <div class=""max-w-xl mx-auto my-6 bg-white rounded-lg shadow-md"">
        //                     <div class=""bg-blue-500 text-white text-center py-4 rounded-t-lg"">
        //                         <h1 class=""text-xl font-bold"">{{header}}</h1>
        //                     </div>
        //                     <div class=""p-6 text-center"">
        //                         <p class=""text-gray-800 text-base"">Hello <b>{{username}}</b>,</p>
        //                         <p class=""text-gray-700 mt-4"">{{content}}</p>
        //                         <div class=""mt-6"">
        //                             <a href=""{{button_link}}"" class=""inline-block bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition duration-300"">
        //                                 {{button_text}}
        //                             </a>
        //                         </div>
        //                     </div>
        //                     <div class=""bg-gray-100 text-center py-3 rounded-b-lg"">
        //                         <p class=""text-gray-500 text-xs"">
        //                             © {{year}} Blood Donation. All rights reserved.
        //                         </p>
        //                         <p>
        //                             <a href=""{{website_link}}"" class=""text-blue-500 text-xs"">Visit our website</a>
        //                         </p>
        //                     </div>
        //                 </div>
        //             </body>
        //             </html>",
        //         MainContent = "Main 1"
        //     },
        //     new EmailTemplate
        //     {
        //         Id = 2,
        //         Header = "Template 2",
        //         Content = @"
        //             <!DOCTYPE html>
        //                 <html>
        //                     <head>
        //                         <meta charset=""UTF-8"" />
        //                         <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
        //                         <title>Email Template</title>
        //                         <link
        //                         href=""https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css""
        //                         rel=""stylesheet""
        //                         />
        //                     </head>
        //                     <body class=""bg-gray-100"">
        //                         <div class=""max-w-xl mx-auto my-6 bg-white rounded-lg shadow-md"">
        //                         <div class=""bg-blue-500 text-white text-center py-4 rounded-t-lg"">
        //                             <h1 class=""text-xl font-bold"">{{header}}</h1>
        //                         </div>
        //                         <div class=""p-6 text-center"">
        //                             <p class=""text-gray-800 text-base"">Hello <b>{{username}}</b>,</p>
        //                             <p class=""text-gray-700 mt-4"">{{content}}</p>
        //                         </div>
        //                         <div class=""bg-gray-100 text-center py-3 rounded-b-lg"">
        //                             <p class=""text-gray-500 text-xs"">
        //                                 © {{year}} Blood Donation. All rights reserved.
        //                             </p>
        //                             <p>
        //                                 <a href=""{{website_link}}"" class=""text-blue-500 text-xs""
        //                                     >Visit our website</a
        //                                 >
        //                             </p>
        //                         </div>
        //                     </div>
        //                 </body>
        //             </html>",
        //         MainContent = "Main 2"
            // }
        // );
    }
}