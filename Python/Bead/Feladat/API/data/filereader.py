import json
from typing import Dict, Any, List
from data.filehandler import load_json as lj
import logging
from utils.logger import log_io
import os

"""
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
"""

logger = logging.getLogger(__name__)

# A JSON fájl elérési útja
JSON_FILE_PATH = os.path.join(os.path.dirname(__file__), "data.json")


@log_io
def load_json() -> Dict[str, Any]:
    with open(JSON_FILE_PATH, "r", encoding="utf-8") as file:
        return json.load(file)


@log_io
def get_user_by_id(user_id: int) -> Dict[str, Any]:
    data = load_json()
    for user in data["Users"]:
        if user["id"] == user_id:
            return user
    else:
        raise ValueError(f"No user was found with id: {user_id}")


@log_io
def get_basket_by_user_id(user_id: int) -> dict[str, Any]:
    data = load_json()
    for basket in data["Baskets"]:
        if basket["user_id"]:
            return basket
    else:
        raise ValueError(f"No basket was found for user id: {user_id}")


@log_io
def get_all_users() -> List[Dict[str, Any]]:
    data = load_json()
    return data["Users"]


@log_io
def get_total_price_of_basket(user_id: int) -> float:
    data = load_json()
    for basket in data["Baskets"]:
        if basket["user_id"] == user_id:
            return sum(item["price"] * item["quantity"] for item in basket["items"])
    else:
        raise ValueError(f"No basket was found for user id: {user_id}")
