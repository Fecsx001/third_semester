import logging
import json

from pydantic import BaseModel, EmailStr, field_validator
from utils.logger import log_io

"""

Útmutató a fájl használatához:

Az osztályokat a schema alapján ki kell dolgozni.

A schema.py az adatok küldésére és fogadására készített osztályokat tartalmazza.
Az osztályokban az adatok legyenek validálva.
 - az int adatok nem lehetnek negatívak.
 - az email mező csak e-mail formátumot fogadhat el.
 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

"""

logger = logging.getLogger(__name__)

ShopName = "Shop API tester shop"


class User(BaseModel):
    id: int
    name: str
    email: EmailStr

    @field_validator("id")
    @log_io
    def validate_id(cls, v):
        if v < 0:
            raise ValueError("ID can't be a negative number")
        return v


class Item(BaseModel):
    item_id: int
    name: str
    brand: str
    price: float
    quantity: int

    @field_validator("item_id", "price", "quantity")
    @log_io
    def validate_non_negative(cls, v):
        if v < 0:
            raise ValueError("Value can't be negative")
        return v


class Basket(BaseModel):
    id: int
    user_id: int
    items: list[Item]

    @field_validator("id", "user_id")
    @log_io
    def validate_ids(cls, v):
        if v < 0:
            raise ValueError("ID can't be negative")
        return v
