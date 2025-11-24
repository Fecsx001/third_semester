import json
import os
from typing import Dict, Any, List
from data.filehandler import load_json as load

'''
Útmutató a féjl használatához:

Felhasználó adatainak lekérdezése:

user_id = 1
user = get_user_by_id(user_id)
print(f"Felhasználó adatai: {user}")

Felhasználó kosarának tartalmának lekérdezése:

user_id = 1
basket = get_basket_by_user_id(user_id)
print(f"Felhasználó kosarának tartalma: {basket}")

Összes felhasználó lekérdezése:

users = get_all_users()
print(f"Összes felhasználó: {users}")

Felhasználó kosarában lévő termékek összárának lekérdezése:

user_id = 1
total_price = get_total_price_of_basket(user_id)
print(f"A felhasználó kosarának összára: {total_price}")

Hogyan futtasd?

Importáld a függvényeket a filehandler.py modulból:

from filereader import (
    get_user_by_id,
    get_basket_by_user_id,
    get_all_users,
    get_total_price_of_basket
)

 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

# A JSON fájl elérési útja
JSON_FILE_PATH = os.path.join(os.getcwd(), "data/data.json")

def get_user_by_id(user_id: int) -> Dict[str, Any]:
    temp = load()
    for user in temp["Users"]:
        if user["id"] == user_id:
            return user
    raise ValueError("Could not find a user")

def get_basket_by_user_id(user_id: int) -> List[Dict[str, Any]]:
    temp = load()
    for basket in temp["Baskets"]:
        if basket["user_id"] == user_id:
            return basket["items"]
    raise ValueError("Could not find a basket")

def get_all_users() -> List[Dict[str, Any]]:
    temp = load()
    return temp["Users"]

def get_total_price_of_basket(user_id: int) -> float:
    temp = load()
    for basket in temp["Baskets"]:
        if basket["user_id"] == user_id:
            total_price = sum(item["price"] * item["quantity"] for item in basket["items"])
            return total_price
    raise ValueError("Could not find a basket")