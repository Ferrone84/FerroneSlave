#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import datetime
import utils

connection = sqlite3.connect(utils.BDD_NAME)
cursor = connection.cursor()

cursor.execute('''DROP TABLE IF EXISTS users''')
cursor.execute('''CREATE TABLE users
			  (id INTEGER PRIMARY KEY AUTOINCREMENT, uid INTEGER NOT NULL UNIQUE, pseudo varchar, prenom varchar, admin boolean)''')

cursor.execute('''DROP TABLE IF EXISTS mangas''')
cursor.execute('''CREATE TABLE mangas
			  (id INTEGER PRIMARY KEY AUTOINCREMENT, titre varchar, scan varchar)''')

cursor.execute('''DROP TABLE IF EXISTS subs''')
cursor.execute('''CREATE TABLE subs
			  (id INTEGER PRIMARY KEY AUTOINCREMENT, user INTEGER CHECK(typeof(user) = 'integer'), manga INTEGER CHECK(typeof(manga) = 'integer'))''')



# values = [(293780484822138881, "ferrone", "nico", True)]
# cursor.executemany("INSERT INTO users (uid,pseudo, prenom, admin) VALUES (?,?,?,?)", values)
# values = [(150338863234154496, "fluttershy", "luc", False)]
# cursor.executemany("INSERT INTO users (uid,pseudo, prenom, admin) VALUES (?,?,?,?)", values)

# values = [("one-piece", "scan numero 999")]
# cursor.executemany("INSERT INTO mangas (titre, scan) VALUES (?,?)", values)
# values = [("bleach", "scan numero 12")]
# cursor.executemany("INSERT INTO mangas (titre, scan) VALUES (?,?)", values)

# values = [(1, 1)]
# cursor.executemany("INSERT INTO subs (user, manga) VALUES (?,?)", values)
# values = [(1, 2)]
# cursor.executemany("INSERT INTO subs (user, manga) VALUES (?,?)", values)
# values = [(2, 1)]
# cursor.executemany("INSERT INTO subs (user, manga) VALUES (?,?)", values)
# connection.commit()

connection.close()
