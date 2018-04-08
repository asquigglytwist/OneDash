public enum ReleaseStages
{
    /// <summary>
    /// Project is in the initial Estimation stage.
    /// </summary>
    Estimation,
    /// <summary>
    /// (Draft) Estimations are completed and a high-level Design is now both implemented and reviewed.
    /// </summary>
    DesignReview,
    /// <summary>
    /// A high-level test plan is created.
    /// </summary>
    TestReview,
    /// <summary>
    /// Budget for the project - like funds, resources, license requirements are being reviewed.
    /// </summary>
    BudgetReview,
    /// <summary>
    /// Budget has been approved and project is now committed to from both ends - Engineering and Management.
    /// </summary>
    PlanCommit,
    /// <summary>
    /// *  Engineering has begun drafting a low-level plan for the agreed upon scope items for the Release.
    /// *  A granular plan is available along with an initial estimate and proposed target date.
    /// *  All stake-holders have agreed upon the proposed plan.
    /// </summary>
    Started,
    /// <summary>
    /// Engineering activities have begun.
    /// </summary>
    InProgress,
    /// <summary>
    /// FeatureComplete - The agreed upon scope (features) is implemented and verified.  Product is now in BugFixing + extended validation stage.
    /// </summary>
    FC,
    /// <summary>
    /// UI related changes are frozen and typically, string changes for localization are not allowed after this stage.
    /// </summary>
    UIFreeze,
    /// <summary>
    /// ZeroBugBounce - At this stage, the Engineering team strives to reach the Zero bugs (either fix them or defer them for a later release :)).
    /// </summary>
    ZBB,
    /// <summary>
    /// A stable build is now available and has been made available for (either open or closed) Beta.
    /// </summary>
    Beta,
    /// <summary>
    /// ReleaseCandidate - Tests have succeeded and stake holders' expectations are satisfied.
    /// </summary>
    RC,
    /// <summary>
    /// ReleasedToSupport - Select few customers are provided the build as an early-preview (the ones who are waiting for specific features / bug-fixes or eager to try the official release but not a Beta quality build).
    /// </summary>
    RTS,
    /// <summary>
    /// ReleasedToManufacturing - The build is now sent for mass-production (i.e., emerging technologies like Floppy Disks, CDs or DVDs - just kidding; This stage is now obselete and mostly not relevant for the industry in 2018).
    /// </summary>
    RTM,
    /// <summary>
    /// ReleasedToWeb - The final build is posted to own / vendor websites from where customers can download it.
    /// </summary>
    RTW,
    /// <summary>
    /// GeneralAvailability - The project is considered as complete and customers are notified.
    /// </summary>
    GA,
    /// <summary>
    /// The project was abandoned mid-way (anywhere before RTW / GA).
    /// </summary>
    Abandoned
}