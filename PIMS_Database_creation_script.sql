CREATE DATABASE `pims` /*!40100 DEFAULT CHARACTER SET utf8 */;

CREATE TABLE `extraction_data` (
  `extractionRecordId` int(11) NOT NULL AUTO_INCREMENT,
  `project_id` int(11) NOT NULL,
  `gcl_id` varchar(45) NOT NULL,
  `sample_id` varchar(45) NOT NULL,
  `sample_type` varchar(45) NOT NULL,
  `nucleic_acid_type` varchar(45) NOT NULL,
  `concentration` double NOT NULL,
  `total_volume` double NOT NULL,
  `total_amount` double NOT NULL,
  `extraction_date` date NOT NULL,
  `extracted_by` varchar(45) NOT NULL,
  `qc_status` varchar(45) NOT NULL,
  PRIMARY KEY (`extractionRecordId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `incoming_project` (
  `temp_project_id` int(11) NOT NULL AUTO_INCREMENT,
  `project_name` varchar(45) DEFAULT NULL,
  `lab_head` varchar(45) NOT NULL,
  `email_id` varchar(45) NOT NULL,
  `contact_name` varchar(45) NOT NULL,
  `contact_email` varchar(45) NOT NULL,
  `contact_phone` varchar(13) NOT NULL,
  `sample_type` varchar(45) NOT NULL,
  `number_of_samples` int(11) NOT NULL,
  `samples_with_core` varchar(45) NOT NULL,
  `species` varchar(45) NOT NULL,
  `service_requested` varchar(45) NOT NULL,
  `downstream_process` varchar(45) DEFAULT NULL,
  `date_submitted` date NOT NULL,
  `comments` longtext,
  PRIMARY KEY (`temp_project_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `library_prep` (
  `libRecordId` int(11) NOT NULL AUTO_INCREMENT,
  `project_id` int(11) NOT NULL,
  `gcl_id` varchar(45) NOT NULL,
  `sample_id` varchar(45) NOT NULL,
  `adapter_id` varchar(45) NOT NULL,
  `adapter_sequence` varchar(45) NOT NULL,
  `input_amount` double NOT NULL,
  `average_size` double NOT NULL,
  `library_concentration` double NOT NULL,
  `nm_concentration` double NOT NULL,
  `total_volume` double NOT NULL,
  `total_amount` double NOT NULL,
  `total_nmoles` double NOT NULL,
  `date_made` date NOT NULL,
  `made_by` varchar(45) NOT NULL,
  `qc_status` varchar(45) NOT NULL,
  PRIMARY KEY (`libRecordId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `new_project` (
  `project_id` int(11) NOT NULL AUTO_INCREMENT,
  `project_name` varchar(100) DEFAULT NULL,
  `investigator_name` varchar(45) NOT NULL,
  `lab_head` varchar(45) NOT NULL,
  `contact_name` varchar(45) NOT NULL,
  `contact_email` varchar(45) NOT NULL,
  `contact_phone` varchar(13) NOT NULL,
  `service_requested` varchar(45) NOT NULL,
  `sample_type` varchar(45) NOT NULL,
  `species` varchar(45) NOT NULL,
  `number_of_samples` int(11) NOT NULL,
  `date_submitted` date NOT NULL,
  `date_samples_submitted` date NOT NULL,
  `current_status` varchar(45) NOT NULL,
  `date_completed` date DEFAULT NULL,
  `downstream_process` varchar(45) DEFAULT NULL,
  `service_cost` double NOT NULL,
  `comments` longtext,
  PRIMARY KEY (`project_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8 COMMENT='						';


CREATE TABLE `sample_detail` (
  `sampleRecordId` int(11) NOT NULL AUTO_INCREMENT,
  `project_id` int(11) NOT NULL,
  `gcl_id` varchar(45) NOT NULL,
  `sample_id` varchar(45) NOT NULL,
  PRIMARY KEY (`sampleRecordId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `sequencing_data` (
  `seqRecordId` int(11) NOT NULL AUTO_INCREMENT,
  `project_id` int(11) NOT NULL,
  `gcl_id` varchar(45) NOT NULL,
  `sample_id` varchar(45) NOT NULL,
  `total_reads` int(11) NOT NULL,
  `unmapped_reads` int(11) NOT NULL,
  `qc_status` varchar(45) NOT NULL,
  `data_delivered` varchar(45) NOT NULL,
  `date_delivered` date NOT NULL,
  PRIMARY KEY (`seqRecordId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) NOT NULL,
  `last_name` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `pi_name` varchar(45) NOT NULL,
  `department` varchar(45) NOT NULL,
  `fund_number` int(11) NOT NULL,
  `cost_center` int(11) NOT NULL,
  `password` varchar(200) NOT NULL,
  `user_type` varchar(45) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8;

CREATE TABLE `capture_data` (
  `captureRecordId` int(11) NOT NULL AUTO_INCREMENT,
  `gcl_id` varchar(45) NOT NULL,
  `project_id` int(11) NOT NULL,
  `sample_id` varchar(45) NOT NULL,
  `capture_type` varchar(45) NOT NULL,
  `input_amount` double NOT NULL,
  `concentration` double NOT NULL,
  `nm_concentration` double NOT NULL,
  `total_volume` double NOT NULL,
  `total_amount` double NOT NULL,
  `date_made` date NOT NULL,
  `made_by` varchar(45) NOT NULL,
  `qc_status` varchar(45) NOT NULL,
  PRIMARY KEY (`captureRecordId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;