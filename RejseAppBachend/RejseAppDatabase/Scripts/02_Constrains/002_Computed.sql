-- Et venskab har ingen retning. UserA_Id og UserB_Id gemmer de valgte brugere,
-- mens UserLow_Id og UserHigh_Id sorterer deres Id'er,
-- så det samme venskab ikke kan gemmes igen i omvendt rækkefølge.

ALTER TABLE dbo.Friendship
ADD UserLow_Id AS
(
    CASE
        WHEN UserA_Id < UserB_Id THEN UserA_Id
        ELSE UserB_Id
    END
) PERSISTED;

ALTER TABLE dbo.Friendship
ADD UserHigh_Id AS
(
    CASE
        WHEN UserA_Id > UserB_Id THEN UserA_Id
        ELSE UserB_Id
    END
) PERSISTED;