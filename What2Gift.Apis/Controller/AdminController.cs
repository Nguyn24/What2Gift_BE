using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Admin.Analytics.GetDashboardStats;
using What2Gift.Application.Admin.BankAccount.CreateBankAccount;
using What2Gift.Application.Admin.BankAccount.UpdateBankAccountQrCode;
using What2Gift.Application.Admin.Memberships.ExtendMembership;
using What2Gift.Application.Admin.Memberships.GetAllMemberships;
using What2Gift.Application.Admin.Payments.GetAllPayments;
using What2Gift.Application.Admin.TopUp.ApproveTopUpTransaction;
using What2Gift.Application.Admin.TopUp.GetAllTopUpTransactions;
using What2Gift.Application.Admin.TopUp.RejectTopUpTransaction;
using What2Gift.Application.Admin.Users.GetAllUsers;
using What2Gift.Application.Admin.Users.ToggleUserStatus;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Apis.Controller;

[Route("api/admin/")]
[ApiController]
[Authorize] // TODO: Add admin role authorization
public class AdminController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUserContext _userContext;
    private readonly IImageUploader _imageUploader;

    public AdminController(ISender mediator, IUserContext userContext, IImageUploader imageUploader)
    {
        _mediator = mediator;
        _userContext = userContext;
        _imageUploader = imageUploader;
    }

    // // Dashboard Analytics
    // [HttpGet("dashboard/stats")]
    // public async Task<IResult> GetDashboardStats(
    //     [FromQuery] DateTime? fromDate,
    //     [FromQuery] DateTime? toDate,
    //     CancellationToken cancellationToken)
    // {
    //     // Normalize to UTC to avoid Unspecified kind issues with timestamptz
    //     DateTime? Normalize(DateTime? dt)
    //     {
    //         if (!dt.HasValue) return null;
    //         var v = dt.Value;
    //         return v.Kind switch
    //         {
    //             DateTimeKind.Utc => v,
    //             DateTimeKind.Local => v.ToUniversalTime(),
    //             _ => DateTime.SpecifyKind(v, DateTimeKind.Utc)
    //         };
    //     }
    //
    //     var query = new GetDashboardStatsQuery
    //     {
    //         FromDate = Normalize(fromDate),
    //         ToDate = Normalize(toDate)
    //     };
    //
    //     Result<DashboardStatsResponse> result = await _mediator.Send(query, cancellationToken);
    //     return result.MatchOk();
    // }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="searchTerm">Search by username or email</param>
    /// <param name="status">User status: 0=Inactive, 1=Active, 2=Suspended</param>
    /// <param name="createdFrom">Filter from date (format: YYYY-MM-DD)</param>
    /// <param name="createdTo">Filter to date (format: YYYY-MM-DD)</param>
    // User Management
    [HttpGet("users")]
    public async Task<IResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? status = null,
        [FromQuery] DateOnly? createdFrom = null,
        [FromQuery] DateOnly? createdTo = null,
        CancellationToken cancellationToken = default)
    {
        DateTime? ToUtcStart(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc) : null;
        DateTime? ToUtcEnd(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc) : null;

        var query = new GetAllUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status.HasValue ? (Domain.Users.UserStatus)status.Value : null,
            CreatedFrom = ToUtcStart(createdFrom),
            CreatedTo = ToUtcEnd(createdTo)
        };

        Result<Page<AdminUserResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("users/{userId}/toggle-status")]
    public async Task<IResult> ToggleUserStatus(
        [FromRoute] Guid userId,
        [FromBody] ToggleUserStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ToggleUserStatusCommand
        {
            UserId = userId,
            Status = (Domain.Users.UserStatus)request.Status
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    /// <summary>
    /// Get all memberships
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="searchTerm">Search by username or email</param>
    /// <param name="isActive">Filter by active status (true/false)</param>
    /// <param name="membershipPlanId">Filter by membership plan ID</param>
    /// <param name="startDateFrom">Filter from start date (format: YYYY-MM-DD)</param>
    /// <param name="startDateTo">Filter to start date (format: YYYY-MM-DD)</param>
    // Membership Management
    [HttpGet("memberships")]
    public async Task<IResult> GetAllMemberships(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? membershipPlanId = null,
        [FromQuery] DateOnly? startDateFrom = null,
        [FromQuery] DateOnly? startDateTo = null,
        CancellationToken cancellationToken = default)
    {
        DateTime? ToUtcStart(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc) : null;
        DateTime? ToUtcEnd(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc) : null;

        var query = new GetAllMembershipsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            IsActive = isActive,
            MembershipPlanId = membershipPlanId,
            StartDateFrom = ToUtcStart(startDateFrom),
            StartDateTo = ToUtcEnd(startDateTo)
        };

        Result<Page<AdminMembershipResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    /// <summary>
    /// Get all payments (top-up approved/rejected and membership registrations)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="searchTerm">Search by username, email, or transaction code</param>
    /// <param name="status">Payment status: 1=Pending, 2=Success, 3=Failed</param>
    /// <param name="createdFrom">Filter from date (format: YYYY-MM-DD)</param>
    /// <param name="createdTo">Filter to date (format: YYYY-MM-DD)</param>
    /// <param name="minAmount">Minimum amount filter</param>
    /// <param name="maxAmount">Maximum amount filter</param>
    /// <param name="paymentType">Filter by payment type: TOP_UP, MEMBERSHIP, or null (all)</param>
    /// <param name="sortBy">Sort field: amount, createdAt, status, paymentMethod, paymentType</param>
    /// <param name="sortOrder">Sort order: 0=Ascending, 1=Descending (default: 1)</param>
    // Payment Management
    [HttpGet("payments")]
    public async Task<IResult> GetAllPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? status = null,
        [FromQuery] DateOnly? createdFrom = null,
        [FromQuery] DateOnly? createdTo = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] decimal? maxAmount = null,
        [FromQuery] string? paymentType = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] int? sortOrder = null,
        CancellationToken cancellationToken = default)
    {
        DateTime? ToUtcStart(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc) : null;
        DateTime? ToUtcEnd(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc) : null;

        var query = new GetAllPaymentsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status.HasValue ? (Domain.Finance.PaymentTransactionStatus)status.Value : null,
            CreatedFrom = ToUtcStart(createdFrom),
            CreatedTo = ToUtcEnd(createdTo),
            MinAmount = minAmount,
            MaxAmount = maxAmount,
            PaymentType = paymentType,
            SortBy = sortBy,
            SortOrder = sortOrder.HasValue ? (Application.Abstraction.Query.SortOrder)sortOrder.Value : Application.Abstraction.Query.SortOrder.Descending
        };

        Result<Page<AdminPaymentResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    // [HttpPut("memberships/{membershipId}/extend")]
    // public async Task<IResult> ExtendMembership(
    //     [FromRoute] Guid membershipId,
    //     [FromBody] ExtendMembershipRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     var command = new ExtendMembershipCommand
    //     {
    //         MembershipId = membershipId,
    //         AdditionalDays = request.AdditionalDays,
    //         Reason = request.Reason
    //     };
    //
    //     Result result = await _mediator.Send(command, cancellationToken);
    //     return result.MatchOk();
    // }

    /// <summary>
    /// Get all pending top-up transactions (only pending status)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="searchTerm">Search by transfer content (nap0000-nap9999), username, or email</param>
    /// <param name="createdFrom">Filter from date (format: YYYY-MM-DD)</param>
    /// <param name="createdTo">Filter to date (format: YYYY-MM-DD)</param>
    /// <param name="minAmount">Minimum amount filter (default: 1000 VND)</param>
    /// <param name="maxAmount">Maximum amount filter</param>
    /// <param name="sortBy">Sort field: amount, createdAt, status</param>
    /// <param name="sortOrder">Sort order: 0=Ascending, 1=Descending (default: 1)</param>
    // Top-Up Management (only pending transactions)
    [HttpGet("topup/transactions")]
    public async Task<IResult> GetAllTopUpTransactions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] DateOnly? createdFrom = null,
        [FromQuery] DateOnly? createdTo = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] decimal? maxAmount = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] int? sortOrder = null,
        CancellationToken cancellationToken = default)
    {
        DateTime? ToUtcStart(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc) : null;
        DateTime? ToUtcEnd(DateOnly? d) => d.HasValue ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc) : null;

        var query = new GetAllTopUpTransactionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            // Status removed - only pending transactions are shown
            CreatedFrom = ToUtcStart(createdFrom),
            CreatedTo = ToUtcEnd(createdTo),
            MinAmount = minAmount,
            MaxAmount = maxAmount,
            SortBy = sortBy,
            SortOrder = sortOrder.HasValue ? (Application.Abstraction.Query.SortOrder)sortOrder.Value : Application.Abstraction.Query.SortOrder.Descending
        };

        Result<Page<AdminTopUpTransactionResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("topup/transactions/{transactionId}/approve")]
    public async Task<IResult> ApproveTopUpTransaction(
        [FromRoute] Guid transactionId,
        [FromBody] ApproveTopUpTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ApproveTopUpTransactionCommand
        {
            TopUpTransactionId = transactionId,
            AdminUserId = _userContext.UserId,
            Note = request.Note
        };

        Result<ApproveTopUpTransactionResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("topup/transactions/{transactionId}/reject")]
    public async Task<IResult> RejectTopUpTransaction(
        [FromRoute] Guid transactionId,
        [FromBody] RejectTopUpTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectTopUpTransactionCommand
        {
            TopUpTransactionId = transactionId,
            AdminUserId = _userContext.UserId,
            Note = request.Note
        };

        Result<RejectTopUpTransactionResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    // Bank Account Management
  

    [HttpPost("bank-account/with-qr-code")]
    [Consumes("multipart/form-data")]
    public async Task<IResult> CreateBankAccountWithQrCode(
        [FromForm] CreateBankAccountWithQrCodeRequest request,
        CancellationToken cancellationToken)
    {
        string? qrCodeUrl = null;

        // Upload QR code if provided
        if (request.QrCodeImage != null && request.QrCodeImage.Length > 0)
        {
            // Validate file type
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(request.QrCodeImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return Results.BadRequest(new { message = "Only PNG, JPG, and JPEG images are allowed" });
            }

            qrCodeUrl = await _imageUploader.UploadImageAsync(
                request.QrCodeImage.OpenReadStream(),
                request.QrCodeImage.FileName,
                "images");
        }

        var command = new CreateBankAccountCommand
        {
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            AccountHolderName = request.AccountHolderName,
            QrCodeUrl = qrCodeUrl
        };

        Result<CreateBankAccountResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    // [HttpPost("bank-account/upload-qr-code")]
    // [Consumes("multipart/form-data")]
    // public async Task<IResult> UploadBankAccountQrCode(
    //     [FromForm] UploadQrCodeRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     if (request.QrCodeImage == null || request.QrCodeImage.Length == 0)
    //     {
    //         return Results.BadRequest(new { message = "QR code image is required" });
    //     }
    //
    //     // Validate file type
    //     var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
    //     var fileExtension = Path.GetExtension(request.QrCodeImage.FileName).ToLowerInvariant();
    //     if (!allowedExtensions.Contains(fileExtension))
    //     {
    //         return Results.BadRequest(new { message = "Only PNG, JPG, and JPEG images are allowed" });
    //     }
    //
    //     // Upload image
    //     var qrCodeUrl = await _imageUploader.UploadImageAsync(
    //         request.QrCodeImage.OpenReadStream(),
    //         request.QrCodeImage.FileName,
    //         "images");
    //
    //     // Update bank account with new QR code URL
    //     var command = new UpdateBankAccountQrCodeCommand
    //     {
    //         QrCodeUrl = qrCodeUrl
    //     };
    //
    //     Result<UpdateBankAccountQrCodeResponse> result = await _mediator.Send(command, cancellationToken);
    //     return result.MatchOk();
    // }

    // [HttpPut("bank-account/qr-code-url")]
    // public async Task<IResult> UpdateBankAccountQrCodeUrl(
    //     [FromBody] UpdateBankAccountQrCodeUrlRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     var command = new UpdateBankAccountQrCodeCommand
    //     {
    //         QrCodeUrl = request.QrCodeUrl
    //     };
    //
    //     Result<UpdateBankAccountQrCodeResponse> result = await _mediator.Send(command, cancellationToken);
    //     return result.MatchOk();
    // }
}

public class ToggleUserStatusRequest
{
    public int Status { get; set; }
}

public class ExtendMembershipRequest
{
    public int AdditionalDays { get; set; }
    public string? Reason { get; set; }
}
