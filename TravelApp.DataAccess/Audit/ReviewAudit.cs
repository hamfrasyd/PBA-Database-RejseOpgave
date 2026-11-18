namespace TravelApp.DataAccess.Audit
{
    /// <summary>Represents one review change recorded by the review audit trigger.</summary>
    public sealed class ReviewAudit
    {
        /// <summary>Gets or sets the audit row identifier.</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the review identifier stored without a foreign key.</summary>
        public int ReviewId { get; set; }

        /// <summary>Gets or sets the change action.</summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>Gets or sets the UTC time of the change.</summary>
        public DateTime ChangedAtUtc { get; set; }

        /// <summary>Gets or sets the login that made the change.</summary>
        public string LoginName { get; set; } = string.Empty;

        /// <summary>Gets or sets the star rating before the change.</summary>
        public byte? OldStars { get; set; }

        /// <summary>Gets or sets the star rating after the change.</summary>
        public byte? NewStars { get; set; }
    }
}
