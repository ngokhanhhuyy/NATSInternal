SET FOREIGN_KEY_CHECKS = 0;

SELECT CONCAT('DROP TABLE IF EXISTS ', GROUP_CONCAT('`', table_name, '`')) INTO @drop_sql
FROM information_schema.tables
WHERE table_schema = DATABASE();

PREPARE stmt FROM @drop_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET FOREIGN_KEY_CHECKS = 1;