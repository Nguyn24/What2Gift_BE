// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using What2Gift.Application.Abstraction.Authentication;
// using What2Gift.Application.Abstraction.Data;
// using What2Gift.Domain.Common.DTO;
//
// namespace What2Gift.Application.Authentication.Register
// {
//     public class CreateUserCommandEventHandler(
//         IMailService mailService,
//         IDbContext context,
//         ITemplateRenderer templateRenderer
//     ) : INotificationHandler<UserCreatedDomainEvent>
//     {
//         public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
//         {
//             var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == notification.UserId, cancellationToken);
//             if (user == null || string.IsNullOrWhiteSpace(user.Email)) return;
//
//             var emailTemplate = await context.EmailTemplates.FirstOrDefaultAsync(e => e.Id == 1, cancellationToken);
//             if (emailTemplate == null) return;
//
//             var currentYear = DateTime.UtcNow.Year;
//             var token = VerifyTokenHelper.Encode(user.Email);
//             var verifyEndpoint = $"/api/user/verify?token={token}";
//             var websiteLink = $"https://blood-donation-dvon.vercel.app/user/verify?token={token}";
//
//             var contentBody = templateRenderer.Render(emailTemplate.Content, new Dictionary<string, string>
//             {
//                 { "header", "Verify Your Account" },
//                 { "username", user.Name },
//                 { "content", "Thank you for registering. Please verify your account by clicking the button below." },
//                 { "year", currentYear.ToString() },
//                 { "website_link", websiteLink }
//             });
//
//             var emailBody = new CreateUserEmailBody
//             {
//                 Content = contentBody,
//                 Header = emailTemplate.Header,
//                 VerifyEndpoint = $"/user/verify?token={token}", 
//                 ButtonName = "Verify Now",
//                 MainContent = emailTemplate.MainContent,
//                 User = user
//             };
//
//             try
//             {
//                 mailService.SendCreateUserEmail(emailBody, user.Email);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"❌ Failed to send email to {user.Email}: {ex.Message}");
//             }
//         }
//     }
// }