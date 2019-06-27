#!/usr/bin/env python
# -*- coding: utf-8 -*-
import codecs
import sqlite3

connection = sqlite3.connect("bdd.db")
cursor = connection.cursor()

cursor.execute('''DROP TABLE IF EXISTS pokemons''')
cursor.execute('''CREATE TABLE pokemons
			  (id INTEGER PRIMARY KEY AUTOINCREMENT, uid INTEGER NOT NULL UNIQUE, urlIcon varchar NOT NULL, name varchar NOT NULL, name_fr varchar NOT NULL, catchRate INTEGER NOT NULL CHECK(typeof(catchRate) = 'integer'), rarityTier INTEGER NOT NULL CHECK(typeof(rarityTier) = 'integer'))''')


f = open("pokemons.p", "r")
for pokemon in f.read().split("<pokemon>\r\n"):
	infos = pokemon.split("\r\n")

	uid = int(infos[0]);
	urlIcon = infos[1].replace("://", "///");
	name = infos[2].lower();
	catchRate = int(infos[3]);
	rarityTier = int(infos[4]);
	name_fr ="tmp"

	print(str(uid)+":"+urlIcon+":"+name+":"+str(catchRate)+":"+str(rarityTier))
	cursor.execute(''' INSERT INTO pokemons (uid, urlIcon, name, name_fr, catchRate, rarityTier) VALUES (?,?,?,?,?,?) ''', [uid, urlIcon, name, name_fr, catchRate, rarityTier])

# aaa = cursor.execute(''' SELECT * FROM pokemons''')
# for uuu in aaa:
# 	print (uuu)
# connection.commit()

i=1
f = open("tmp.txt", "r")
for line in f.read().split("\r\n"):
	english_name = line.split("\t")[1].lower()
	french_name = line.split("\t")[3].lower()

	cursor.execute(''' UPDATE pokemons SET name_fr=? WHERE name=? ''', [french_name, english_name])

	# print(english_name + "/" + french_name)
	# result = cursor.execute(''' SELECT * FROM pokemons WHERE name=?''', [english_name])
	# for a in result:
	# 	print(str(i) + ": "+str(a))
	# 	i = i+1

connection.commit()
# aaa1 = cursor.execute(''' UPDATE pokemons SET name='porygon2' WHERE id=233 ''')
# aaa = cursor.execute(''' SELECT * FROM pokemons WHERE id=233''')
aaa = cursor.execute(''' SELECT * FROM pokemons''')
for uuu in aaa:
	print (uuu)
connection.close()





# string allText = System.IO.File.ReadAllText(POKEMONS_FILE_NAME);
# string[] pokemons = allText.Split("<pokemon>\r\n");

# foreach (string pokemon in pokemons) {
# 	string[] infos = pokemon.Split("\r\n");
	
# 	int id = Int32.Parse(infos[0]);
# 	string urlIcon = infos[1];
# 	string name = infos[2].ToLower();
# 	int catchRate = Int32.Parse(infos[3]);
# 	int rarityTier = Int32.Parse(infos[4]);

# 	Program.database.addPokemon(id, urlIcon, name, catchRate, rarityTier);
# }




#utiliser la bdd actuelle pour éviter de faire nimp
#maintenant il faut refaire la table pokemons pour ajouter la colonne name_fr
#refaire le script c# mais en python et du coup ajouter au passage le name_fr comme au dessus ^

#une fois reload, refaire ce test pour vérif qu'il catch bien tout =>
	# connection = sqlite3.connect("bdd.db")
	# cursor = connection.cursor()

	# i=1
	# f = open("tmp.txt", "r")
	# for line in f.read().split("\n"):
	# 	english_name = line.split("\t")[1].lower()
	# 	french_name = line.split("\t")[3].lower()

	# 	print(english_name + "/" + french_name)
	# 	result = cursor.execute(''' SELECT * FROM pokemons WHERE name=?''', [english_name])
	# 	for a in result:
	# 		print(str(i) + ": "+str(a))
	# 		i = i+1
	# connection.close()