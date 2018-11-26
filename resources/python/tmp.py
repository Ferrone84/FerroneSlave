#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3
import datetime
import utils

# connection = sqlite3.connect(utils.BDD_NAME)
# cursor = connection.cursor()

# cursor.execute('''DROP TABLE IF EXISTS pokemons''')
# cursor.execute('''CREATE TABLE pokemons
# 			  (id INTEGER PRIMARY KEY AUTOINCREMENT, uid INTEGER NOT NULL UNIQUE, urlIcon varchar NOT NULL, name varchar NOT NULL, catchRate INTEGER NOT NULL CHECK(typeof(catchRate) = 'integer'), rarityTier INTEGER NOT NULL CHECK(typeof(rarityTier) = 'integer'))''')

# connection.close()
