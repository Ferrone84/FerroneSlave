#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs

#Globales
BDD_NAME = 'resources/binaries/bdd.db'


#Functions
def decryptQuery(query):
	return query.replace(":", " ")


