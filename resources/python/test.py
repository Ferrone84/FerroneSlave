#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import datetime

connection = sqlite3.connect('resources/bdd.db')
cursor = connection.cursor()

cursor.execute('''DROP TABLE IF EXISTS rams''')
cursor.execute('''CREATE TABLE rams
			  (id_ram INTEGER PRIMARY KEY AUTOINCREMENT, cur_date varchar, computer_name varchar, total_ram INTEGER, percent_ram_used real)''')

computer_name = "ferrone"
cur_date = datetime.datetime.now()
values = [(cur_date,computer_name, 10000, 25)]
cursor.executemany("INSERT INTO rams (cur_date,computer_name, total_ram, percent_ram_used) VALUES (?,?,?,?)", values)


connection.commit()
connection.close()

print ("\033[32;1mLes tables ont bien été initialisées/cleen.\033[0m")

print("lel")