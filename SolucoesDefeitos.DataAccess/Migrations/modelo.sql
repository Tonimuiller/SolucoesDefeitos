-- MySQL Script generated by MySQL Workbench
-- Sat Oct 30 14:23:12 2021
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema solucaodefeito
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema solucaodefeito
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `solucaodefeito` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `solucaodefeito` ;

-- -----------------------------------------------------
-- Table `solucaodefeito`.`anomaly`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`anomaly` (
  `anomalyid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `summary` VARCHAR(100) NOT NULL,
  `description` VARCHAR(1000) NULL DEFAULT NULL,
  `repairsteps` VARCHAR(1000) NULL DEFAULT NULL,
  PRIMARY KEY (`anomalyid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `solucaodefeito`.`manufacturer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`manufacturer` (
  `manufacturerid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `enabled` TINYINT NOT NULL,
  `name` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`manufacturerid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `solucaodefeito`.`productgroup`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`productgroup` (
  `productgroupid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `enabled` TINYINT NOT NULL,
  `description` VARCHAR(100) NOT NULL,
  `fatherproductgroupid` INT NULL DEFAULT NULL,
  PRIMARY KEY (`productgroupid`),
  INDEX `fk_fatherproductgroup_idx` (`fatherproductgroupid` ASC) VISIBLE,
  CONSTRAINT `fk_fatherproductgroup`
    FOREIGN KEY (`fatherproductgroupid`)
    REFERENCES `solucaodefeito`.`productgroup` (`productgroupid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `solucaodefeito`.`product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`product` (
  `productid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `enabled` TINYINT NOT NULL,
  `manufacturerid` INT NULL DEFAULT NULL,
  `productgroupid` INT NULL DEFAULT NULL,
  `name` VARCHAR(100) NOT NULL,
  `code` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`productid`),
  INDEX `fk_product_manufacturer_idx` (`manufacturerid` ASC) VISIBLE,
  INDEX `fk_product_productgroup_idx` (`productgroupid` ASC) VISIBLE,
  CONSTRAINT `fk_product_manufacturer`
    FOREIGN KEY (`manufacturerid`)
    REFERENCES `solucaodefeito`.`manufacturer` (`manufacturerid`),
  CONSTRAINT `fk_product_productgroup`
    FOREIGN KEY (`productgroupid`)
    REFERENCES `solucaodefeito`.`productgroup` (`productgroupid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `solucaodefeito`.`anomalyproductspecification`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`anomalyproductspecification` (
  `anomalyproductspecificationid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `anomalyid` INT NOT NULL,
  `productid` INT NOT NULL,
  `manufactureyear` INT NULL DEFAULT NULL,
  PRIMARY KEY (`anomalyproductspecificationid`),
  INDEX `fk_anomalyproductspecification_anomaly_idx` (`anomalyid` ASC) VISIBLE,
  INDEX `fk_anomalyproductspecification_product_idx` (`productid` ASC) VISIBLE,
  CONSTRAINT `fk_anomalyproductspecification_anomaly`
    FOREIGN KEY (`anomalyid`)
    REFERENCES `solucaodefeito`.`anomaly` (`anomalyid`),
  CONSTRAINT `fk_anomalyproductspecification_product`
    FOREIGN KEY (`productid`)
    REFERENCES `solucaodefeito`.`product` (`productid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `solucaodefeito`.`attachment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solucaodefeito`.`attachment` (
  `attachmentid` INT NOT NULL AUTO_INCREMENT,
  `creationdate` DATETIME NOT NULL,
  `updatedate` DATETIME NULL DEFAULT NULL,
  `description` VARCHAR(100) NOT NULL,
  `category` INT NOT NULL,
  `anomalyid` INT NOT NULL,
  `storage` VARCHAR(1000) NOT NULL,
  PRIMARY KEY (`attachmentid`),
  INDEX `fk_attachment_anomaly_idx` (`anomalyid` ASC) VISIBLE,
  CONSTRAINT `fk_attachment_anomaly`
    FOREIGN KEY (`anomalyid`)
    REFERENCES `solucaodefeito`.`anomaly` (`anomalyid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;