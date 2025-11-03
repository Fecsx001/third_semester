from pydantic import BaseModel, EmailStr, Field, field_validator
from typing import List
import re

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

ShopName = "Bolt"


class Item(BaseModel):
    item_id: int = Field(..., description="The unique ID of the item")
    name: str = Field(..., description="The name of the item")
    brand: str = Field(..., description="The brand of the item")
    price: float = Field(
        ..., ge=0, description="The price of the item, must be non-negative"
    )
    quantity: int = Field(
        ..., ge=0, description="The quantity of the item, must be non-negative"
    )

    @field_validator("item_id", "quantity")
    def validate_non_negative(cls, value):
        if value < 0:
            raise ValueError("Value cannot be negative")
        return value


class Basket(BaseModel):
    id: int = Field(..., description="The unique ID of the basket")
    user_id: int = Field(..., description="The ID of the user who owns the basket")
    items: List[Item] = Field(
        default_factory=list, description="List of items in the basket"
    )

    @field_validator("id", "user_id")
    def validate_non_negative(cls, value):
        if value < 0:
            raise ValueError("Value cannot be negative")
        return value


class User(BaseModel):
    id: int = Field(..., description="The unique ID of the user")
    name: str = Field(..., description="The name of the user")
    email: EmailStr = Field(..., description="The email address of the user")

    @field_validator("id")
    def validate_non_negative(cls, value):
        if value < 0:
            raise ValueError("Value cannot be negative")
        return value

    @field_validator("email")
    def validate_email(cls, value):
        if not re.match(r"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", value):
            raise ValueError("The email must be valid email format")
        return value.lower()
