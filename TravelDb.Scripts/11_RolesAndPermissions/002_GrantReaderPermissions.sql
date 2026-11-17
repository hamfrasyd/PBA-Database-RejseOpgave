-- Giver reader adgang til samlet statistik pr. destination.
-- Viewet medregner alle rejser i databasen.

GRANT SELECT ON OBJECT::[dbo].[vw_DestinationStats]
    TO [traveldb_reader];