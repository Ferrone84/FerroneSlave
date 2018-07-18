#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import utils

connection = sqlite3.connect(utils.BDD_NAME)
cursor = connection.cursor()
display = ""

results = cursor.execute(''' SELECT * FROM users ''')

for result in results:
	display += str(result)+":"

results = cursor.execute(''' SELECT * FROM mangas ''')

for result in results:
	display += str(result)+":"

results = cursor.execute(''' SELECT * FROM subs ''')

for result in results:
	display += str(result)+":"


results = cursor.execute(''' SELECT titre FROM subs JOIN mangas ON(subs.manga=mangas.id) JOIN users ON(subs.user=users.id) WHERE pseudo='ferrone' ''')

for result in results:
	display += str(result)+":"

connection.close()

print (display)