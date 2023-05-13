CREATE OR ALTER PROCEDURE [dbo].[GetFileTree]
	@ParentId BIGINT,
	@IncludeFolder TINYINT
AS
BEGIN
	WITH 
		recursive_cte AS 
		(
			SELECT
				Files.Id,
				Files.Name,
				Files.IsFolder,
				Files.Hash,
				CAST(Files.Name as NVARCHAR(MAX)) as path
			FROM Files
			WHERE ParentFileId = @ParentId

			UNION ALL

			SELECT
				Files.Id,
				Files.Name,
				Files.IsFolder,
				Files.Hash,
				recursive_cte.path + '/' + Files.Name
			FROM Files
			INNER JOIN recursive_cte ON recursive_cte.Id = Files.ParentFileId
		)
	SELECT * FROM recursive_cte
	WHERE IsFolder = @IncludeFolder OR IsFolder = 0
END;