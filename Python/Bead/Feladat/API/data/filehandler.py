import json
from typing import Dict, Any
import os
import logging
from utils.logger import log_io
from pydantic import BaseModel

"""
Útmutató a fájl függvényeinek a használatához

Új felhasználó hozzáadása:

new_user = {
    "id": 4,  # Egyedi felhasználó azonosító
    "name": "Szilvás Szabolcs",
    "email": "szabolcs@plumworld.com"
}

Felhasználó hozzáadása a JSON fájlhoz:

add_user(new_user)

Hozzáadunk egy új kosarat egy meglévő felhasználóhoz:

new_basket = {
    "id": 104,  # Egyedi kosár azonosító
    "user_id": 2,  # Az a felhasználó, akihez a kosár tartozik
    "items": []  # Kezdetben üres kosár
}

add_basket(new_basket)

Új termék hozzáadása egy felhasználó kosarához:

user_id = 2
new_item = {
    "item_id": 205,
    "name": "Szilva",
    "brand": "Stanley",
    "price": 7.99,
    "quantity": 3
}

Termék hozzáadása a kosárhoz:

add_item_to_basket(user_id, new_item)

Hogyan használd a fájlt?

Importáld a függvényeket a filehandler.py modulból:

from filehandler import (
    add_user,
    add_basket,
    add_item_to_basket,
)

 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

"""

logger = logging.getLogger(__name__)

# A JSON fájl elérési útja
JSON_FILE_PATH = os.path.join(os.getcwd(), "data/data.json")
print(JSON_FILE_PATH)


@log_io
def load_json() -> Dict[str, Any]:
    with open(JSON_FILE_PATH, "r", encoding="utf-8") as file:
        return json.load(file)


@log_io
def save_json(data: Dict[str, Any]) -> None:
    with open(JSON_FILE_PATH, "w", encoding="utf-8") as file:
        json.dump(data, file, indent=2, ensure_ascii=False)


@log_io
def add_user(user: Dict[str, Any]) -> None:
    data = load_json()
    data["Users"].append(user)
    save_json(data)


@log_io
def add_basket(basket: Dict[str, Any]) -> None:
    data = load_json()
    data["Baskets"].append(basket)
    save_json(data)


@log_io
def add_item_to_basket(user_id: int, item: Dict[str, Any]) -> None:
    data = load_json()
    for basket in data["Baskets"]:
        if basket["user_id"] == user_id:
            basket["items"].append(item)
            break
    else:
        raise ValueError(f"No basket was found for user id: {user_id}")
    save_json(data)
