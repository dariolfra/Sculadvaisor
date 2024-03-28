-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versione server:              10.4.32-MariaDB - mariadb.org binary distribution
-- S.O. server:                  Win64
-- HeidiSQL Versione:            11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dump della struttura del database schooladvisor
CREATE DATABASE IF NOT EXISTS `schooladvisor` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `schooladvisor`;

-- Dump della struttura di tabella schooladvisor.reviews
CREATE TABLE IF NOT EXISTS `reviews` (
  `reviewID` int(11) NOT NULL AUTO_INCREMENT,
  `tripID` int(11) DEFAULT NULL,
  `userID` int(11) DEFAULT NULL,
  `reviewState` varchar(50) DEFAULT NULL,
  `reviewRating` int(11) DEFAULT NULL,
  `reviewComment` text DEFAULT NULL,
  PRIMARY KEY (`reviewID`),
  KEY `FK__trips` (`tripID`),
  KEY `FK__users` (`userID`),
  CONSTRAINT `FK__trips` FOREIGN KEY (`tripID`) REFERENCES `trips` (`tripID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK__users` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella schooladvisor.reviews: ~0 rows (circa)
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;

-- Dump della struttura di tabella schooladvisor.trips
CREATE TABLE IF NOT EXISTS `trips` (
  `tripID` int(11) NOT NULL AUTO_INCREMENT,
  `tripName` varchar(50) DEFAULT NULL,
  `tripDate` datetime DEFAULT NULL,
  `tripDescription` text DEFAULT NULL,
  `image` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`tripID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella schooladvisor.trips: ~3 rows (circa)
/*!40000 ALTER TABLE `trips` DISABLE KEYS */;
REPLACE INTO `trips` (`tripID`, `tripName`, `tripDate`, `tripDescription`, `image`) VALUES
	(1, 'uscita teatro Treviso', '2024-03-25 13:00:00', 'visione teatrale di JOJO parte 3 Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi porta posuere justo a scelerisque. Suspendisse eleifend sit amet turpis sit amet faucibus. Donec vel interdum sapien. Sed sodales placerat magna et elementum. Nulla nec tincidunt lorem. Fusce tempor justo eget maximus viverra. Nunc vitae urna imperdiet, pretium dui quis, tristique neque.', '/img/goofyCat.jpeg'),
	(2, 'mostra d\'arte: "culi sudati"', '2024-04-20 10:00:00', 'galleria contemporanea con quadri animati Phasellus eget maximus libero, vel auctor arcu. Suspendisse potenti. Ut in luctus enim. Nullam vel aliquam nulla. Phasellus placerat facilisis felis, non facilisis metus fringilla vitae. Nulla eu arcu vel arcu fermentum convallis eget at quam. Curabitur eleifend vitae tortor id maximus. Mauris auctor purus vel nibh tempor, ut rutrum velit lobortis. Sed nec orci sed nisl tristique interdum. Quisque sed quam mauris.', '/img/foglioProtocollo.gif'),
	(3, 'uscita al cinema esperia ', '2023-03-28 09:15:00', 'visione film "eepy cat" Morbi et risus mi. Curabitur fermentum massa et magna finibus, sit amet imperdiet elit luctus. Aenean aliquam urna ac quam dictum cursus. Sed eget erat urna. Aenean fermentum nisi nec urna gravida ultrices. Etiam congue magna augue, a placerat nulla egestas ut. In malesuada iaculis elit, pretium sagittis tortor gravida vel. Aliquam pellentesque vulputate nunc at congue. Maecenas vel turpis iaculis mauris rhoncus venenatis ut eu nulla.', '/img/pipone.jpeg');
/*!40000 ALTER TABLE `trips` ENABLE KEYS */;

-- Dump della struttura di tabella schooladvisor.users
CREATE TABLE IF NOT EXISTS `users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella schooladvisor.users: ~0 rows (circa)
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
