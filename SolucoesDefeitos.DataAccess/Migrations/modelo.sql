-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: solucaodefeito
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `anomaly`
--

DROP TABLE IF EXISTS `anomaly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `anomaly` (
  `anomalyid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `summary` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `repairsteps` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`anomalyid`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `anomalyproductspecification`
--

DROP TABLE IF EXISTS `anomalyproductspecification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `anomalyproductspecification` (
  `anomalyproductspecificationid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `anomalyid` int NOT NULL,
  `productid` int NOT NULL,
  `manufactureyear` int DEFAULT NULL,
  PRIMARY KEY (`anomalyproductspecificationid`),
  KEY `fk_anomalyproductspecification_anomaly_idx` (`anomalyid`),
  KEY `fk_anomalyproductspecification_product_idx` (`productid`),
  CONSTRAINT `fk_anomalyproductspecification_anomaly` FOREIGN KEY (`anomalyid`) REFERENCES `anomaly` (`anomalyid`),
  CONSTRAINT `fk_anomalyproductspecification_product` FOREIGN KEY (`productid`) REFERENCES `product` (`productid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `attachment`
--

DROP TABLE IF EXISTS `attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachment` (
  `attachmentid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `category` int NOT NULL,
  `anomalyid` int NOT NULL,
  `storage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`attachmentid`),
  KEY `fk_attachment_anomaly_idx` (`anomalyid`),
  CONSTRAINT `fk_attachment_anomaly` FOREIGN KEY (`anomalyid`) REFERENCES `anomaly` (`anomalyid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `manufacturer`
--

DROP TABLE IF EXISTS `manufacturer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturer` (
  `manufacturerid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `enabled` tinyint NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`manufacturerid`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `productid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `enabled` tinyint NOT NULL,
  `manufacturerid` int DEFAULT NULL,
  `productgroupid` int DEFAULT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`productid`),
  KEY `fk_product_manufacturer_idx` (`manufacturerid`),
  KEY `fk_product_productgroup_idx` (`productgroupid`),
  CONSTRAINT `fk_product_manufacturer` FOREIGN KEY (`manufacturerid`) REFERENCES `manufacturer` (`manufacturerid`),
  CONSTRAINT `fk_product_productgroup` FOREIGN KEY (`productgroupid`) REFERENCES `productgroup` (`productgroupid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `productgroup`
--

DROP TABLE IF EXISTS `productgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productgroup` (
  `productgroupid` int NOT NULL AUTO_INCREMENT,
  `creationdate` datetime NOT NULL,
  `updatedate` datetime DEFAULT NULL,
  `enabled` tinyint NOT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fatherproductgroupid` int DEFAULT NULL,
  PRIMARY KEY (`productgroupid`),
  KEY `fk_fatherproductgroup_idx` (`fatherproductgroupid`),
  CONSTRAINT `fk_fatherproductgroup` FOREIGN KEY (`fatherproductgroupid`) REFERENCES `productgroup` (`productgroupid`)
) ENGINE=InnoDB AUTO_INCREMENT=127 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'solucaodefeito'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-06 10:50:47
