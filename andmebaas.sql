create database Kino2
use Kino2




-- Creating tables based on the provided diagram

-- Table: Zanrid (Genres)
CREATE TABLE Zanrid (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nimetus NVARCHAR(255) NOT NULL
);

-- Table: Rezisor (Directors)
CREATE TABLE Rezisor (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nimi NVARCHAR(255) NOT NULL,
    PerekonaNimi NVARCHAR(255) NOT NULL
);

-- Table: Vanus (Age Restrictions)
CREATE TABLE Vanus (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nimetus NVARCHAR(255) NOT NULL
);

-- Table: Film
CREATE TABLE Film (
    ID INT PRIMARY KEY IDENTITY(1,1),
    FilmiNimi NVARCHAR(255) NOT NULL,
    Kirjeldus NVARCHAR(MAX),
    Pikkus INT NOT NULL, -- duration in minutes
    VyljalaskeKuupaev DATE,
    Poster NVARCHAR(255),
    RezisoorID INT,
    ZanrID INT,
    VanusID INT,
    FOREIGN KEY (RezisoorID) REFERENCES Rezisor(ID),
    FOREIGN KEY (ZanrID) REFERENCES Zanrid(ID),
    FOREIGN KEY (VanusID) REFERENCES Vanus(ID)
);

-- Table: Saal (Halls)
CREATE TABLE Saal (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nimetus NVARCHAR(255) NOT NULL,
    Kino NVARCHAR(255) NOT NULL
);

-- Table: Koht (Seats)
CREATE TABLE Koht (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Rida INT NOT NULL, -- row number
    Veerus INT NOT NULL -- seat number
);

-- Table: Konto (Accounts)
CREATE TABLE Konto (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nimi NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Parool NVARCHAR(255) NOT NULL
);

-- Table: Seanss (Sessions)
CREATE TABLE Seanss (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Aeg DATETIME NOT NULL,
    FilmID INT NOT NULL,
    SaalID INT NOT NULL,
    FOREIGN KEY (FilmID) REFERENCES Film(ID),
    FOREIGN KEY (SaalID) REFERENCES Saal(ID)
);

-- Table: Pilet (Tickets)
CREATE TABLE Pilet (
    ID INT PRIMARY KEY IDENTITY(1,1),
    KohtID INT NOT NULL,
    SeanssID INT NOT NULL,
    KontoID INT,
    FOREIGN KEY (KohtID) REFERENCES Koht(ID),
    FOREIGN KEY (SeanssID) REFERENCES Seanss(ID),
    FOREIGN KEY (KontoID) REFERENCES Konto(ID)
);








-- Inserting sample data into Zanrid (Genres)
INSERT INTO Zanrid (Nimetus) VALUES
('Action'),
('Drama'),
('Comedy'),
('Horror'),
('Sci-Fi');

-- Inserting sample data into Rezisor (Directors)
INSERT INTO Rezisor (Nimi, PerekonaNimi) VALUES
('Christopher', 'Nolan'),
('Quentin', 'Tarantino'),
('Steven', 'Spielberg'),
('Ridley', 'Scott'),
('James', 'Cameron');

-- Inserting sample data into Vanus (Age Restrictions)
INSERT INTO Vanus (Nimetus) VALUES
('G'),
('PG'),
('PG-13'),
('R'),
('NC-17');

-- Inserting sample data into Film
INSERT INTO Film (FilmiNimi, Kirjeldus, Pikkus, VyljalaskeKuupaev, Poster, RezisoorID, ZanrID, VanusID) VALUES
('Inception', 'A mind-bending thriller', 148, '2010-07-16', 'inception.jpg', 1, 1, 3),
('Pulp Fiction', 'A series of interconnected stories', 154, '1994-10-14', 'pulp_fiction.jpg', 2, 2, 4),
('Jurassic Park', 'Dinosaurs are back!', 127, '1993-06-11', 'jurassic_park.jpg', 3, 5, 3),
('Alien', 'A terrifying space horror', 117, '1979-05-25', 'alien.jpg', 4, 4, 4),
('Avatar', 'A visually stunning sci-fi epic', 162, '2009-12-18', 'avatar.jpg', 5, 5, 3);

-- Inserting sample data into Saal (Halls)
INSERT INTO Saal (Nimetus, Kino) VALUES
('Hall 1', 'Cinema One'),
('Hall 2', 'Cinema One'),
('IMAX', 'Cinema One'),
('VIP Hall', 'Cinema Two'),
('Regular', 'Cinema Two');

-- Inserting sample data into Koht (Seats)
INSERT INTO Koht (Rida, Veerus) VALUES
(1, 1),
(1, 2),
(2, 1),
(2, 2),
(3, 1);

-- Inserting sample data into Konto (Accounts)
INSERT INTO Konto (Nimi, Email, Parool) VALUES
('John Doe', 'john.doe@example.com', 'password123'),
('Jane Smith', 'jane.smith@example.com', 'securepass'),
('Alice Johnson', 'alice.j@example.com', 'alice123'),
('Bob Brown', 'bob.brown@example.com', 'password1'),
('Charlie White', 'charlie.w@example.com', 'mypassword');

-- Inserting sample data into Seanss (Sessions)
INSERT INTO Seanss (Aeg, FilmID, SaalID) VALUES
('2024-12-13 18:00:00', 1, 1),
('2024-12-13 20:00:00', 2, 2),
('2024-12-14 17:30:00', 3, 3),
('2024-12-14 19:00:00', 4, 4),
('2024-12-15 16:00:00', 5, 5);

-- Inserting sample data into Pilet (Tickets)
INSERT INTO Pilet (KohtID, SeanssID, KontoID) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 2, 3),
(4, 3, 4),
(5, 4, 5);
