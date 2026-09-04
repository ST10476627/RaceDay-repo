--Creating a database
CREATE DATABASE RaceDay;

--beigninning of the table creation
Create table [Role] (
RoleID int identity(1,1) primary key,
RoleName varchar(100) not null unique
);


CREATE TABLE [User] (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName varchar (100) NOT NULL,
    LastName varchar(100) NOT NULL,
    Email varchar (100) NOT NULL UNIQUE,
    password_hash varchar(255) NOT NULL,
    RoleID int NOT NULL,
    foreign key (RoleID) references [Role](RoleID)
);


Create table [Event](
EventID int identity (1,1) primary key,
EventName varchar(100) not null,
Event_Discrip varchar(250) not null,
EventDate Datetime not null,
Distance Decimal(5,2) NOT NULL,
EventType varchar(80) not null,
Location varchar (250) not null, 
organiserID int not null,
foreign key (organiserID) references [User](UserId)
);

Create table Category (
    CategoryID int identity (1,1) primary key,
    CategoryName varchar(100) not null,
    CategoryType varchar (250) not null,
    EventID int ,
   foreign key (EventID) references [Event](EventID)
);

Create table Enrolment (
    EnrolmentID int identity (1,1) primary key,
    ParticipantID int not null,
    EnrolmentDate datetime not null,
    EventID int not null,
    CategoryID int not null,
    Enrolment_status varchar (100) not null default 'pending...',
    foreign key (ParticipantID) references [User](UserId),
    foreign key (EventID) references [Event](EventID),
    foreign key (CategoryID) references Category(CategoryID)
);

CREATE TABLE Results (
    ResultsID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID int not null unique,
    FinishTime time not null,
    FinishPosition int not null,
    FOREIGN KEY (EnrolmentID) REFERENCES Enrolment(EnrolmentID)
);
-- end of table creation 

-- on the first insert to diplay which roles get what ID
INSERT INTO [Role] (RoleName) VALUES 
('Organiser'), 
('Participant');
 -- this captures the names of the peoople doing the system 
INSERT INTO [User] (FirstName, LastName, Email, password_hash, RoleID) VALUES 
( 'David', 'Nzapa', 'Babydaiz5@gmail.com', '$2a$11$0KjV/lX.K90C2W7wT8oD3e3O9mK2E4.k7vT1W3y4Z5A6B7C8D9E0F', 1),
( 'Michael', 'Olise', 'm-olise@gmail.com', '$2y$11$pggcd7vyfB3WyKQSmmKAG.Z510f2Ew59icYZpQUpd1pgczmgyQ6k.', 1),
('Ogorogile', 'Shikwambana', 'Ogorogilegoro@gmail.com', '$2y$11$DfWf81maA0sIaLvh0prayuwYq/fu66g565e8L8mVKgTLJ4veJHGMa', 2),
('Vhukhudo', 'Ramabulana', 'RamabulanaV8@gmail.com', '$2y$11$8/mIvualoegFRShQjtWrbOBvSv1AqNIjbKK1tDjChFa9M4/v8Lika', 2),
( 'Sally', 'Ruchezme', 'RuchezmeQueen33@yahoo.com', '$2y$10$MyYsPcCB903.tP9rJ2Lv..I6AYq02lZotjR6KmJb6fJFOIDVcpoFK', 2),
('Maria', 'Robinson', 'Maria.R20@gmail.com', '$2y$11$ZdXS5g4Z5novdZQdOdoauOSUw9DK8XWfZI4bMNcy.FPIaQk3PHAa2', 2),
('Mike', 'Hunter', 'HunterIceboy5@yahoo.com', '$2y$11$po.ohMF6GMl19EjuLe2po.ukvfEfU0d7VD6740GAex7wn/SuuXhne', 2);


INSERT INTO Event (EventName, Event_Discrip, EventDate, Distance, EventType, Location, organiserID) VALUES 
('Two Oceans Marathon', 'World-renowned ultra-marathon along the cape peninsula', '2027-06-13 06:00:00', 56.00, 'Run', 'Cape Town, Western Cape', 1),
('947 Ride Joburg', 'South Africa annual cycling race held on closed roads across Johannesburg', '2028-12-12 05:30:00', 97.00, 'Cycle', 'FNB Stadium', 2),
('Comrades Marathon', 'The ultimate human race of 89km between Durban and Pietermaritzburg', '2027-06-13 05:30:00', 89.00, 'Run', 'Durban to Pietermaritzburg', 1),
('Cape Town Cycle Tour', 'Timed cycle race across Cape Peninsula', '2027-03-14 06:15:00', 109.00, 'Cycle', 'Grand Parade', 2);

INSERT INTO Category (CategoryName, CategoryType, EventID) VALUES
('56km', 'Distance', 1),
('97km', 'Distance', 2),
('89km', 'Distance', 3),
('109km', 'Distance',4);

INSERT INTO Enrolment(ParticipantID, EnrolmentDate, EventID, CategoryID, Enrolment_status)VALUES
(3, '2026-09-10 09:00:00', 1, 1, 'Confirmed'),
(4, '2026-09-11 10:30:00', 1, 2, 'Confirmed'),
(5, '2026-09-12 11:00:00', 2, 2, 'Pending'),
(6, '2026-09-13 12:15:00', 3, 3, 'Confirmed'),
(7, '2026-09-14 14:00:00', 4, 4, 'Pending');

INSERT INTO Results (EnrolmentID, FinishTime, FinishPosition) VALUES
(5, '04:45:30', 102),
(6, '05:12:15', 215),
(8, '07:30:00', 88);  

