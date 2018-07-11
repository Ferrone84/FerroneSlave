#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import datetime

connection = sqlite3.connect('resources/bdd.db')
cursor = connection.cursor()

results = cursor.execute(''' SELECT * FROM rams ''')

for result in results:
	print(result)


connection.close()