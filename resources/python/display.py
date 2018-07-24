#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import utils

connection = sqlite3.connect(utils.BDD_NAME)
cursor = connection.cursor()
display = ""

results = cursor.execute(''' SELECT * FROM users ''')

display += "```asciidoc:[users]```:"
for result in results:
	display += str(result)+":"

results = cursor.execute(''' SELECT * FROM mangas ''')

display += ":```asciidoc:[mangas]```:"
for result in results:
	display += str(result)+":"

results = cursor.execute(''' SELECT * FROM subs ''')

display += ":```asciidoc:[subs]```:"
for result in results:
	display += str(result)+":"

connection.close()

print (display)