#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import sys
import utils

connection = sqlite3.connect(utils.BDD_NAME)
cursor = connection.cursor()
display = ""

query = utils.decryptQuery(sys.argv[1])
values = []
if (len(sys.argv) > 2):
	values = (sys.argv[2]).split(":")

results = cursor.execute(query, values)

for result in results:
	display += str(result)+":"

connection.commit();
connection.close()

print (display[:-1])