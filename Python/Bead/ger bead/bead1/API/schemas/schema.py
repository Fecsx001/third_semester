from pydantic import BaseModel, EmailStr, field_validator

'''

Útmutató a fájl használatához:

Az osztályokat a schema alapján ki kell dolgozni.

A schema.py az adatok küldésére és fogadására készített osztályokat tartalmazza.
Az osztályokban az adatok legyenek validálva.
 - az int adatok nem lehetnek negatívak.
 - az email mező csak e-mail formátumot fogadhat el.
 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

ShopName='Bolt'

class Item(BaseModel): # elsőnek kell lennie, vagy nem látja a basket listje
    item_id : int
    name : str
    brand : str
    price : float
    quantity : int

    @field_validator("item_id", "quantity")
    def validate_non_negative(cls, value):
        if value < 0:
            raise ValueError("Value must be bigger than 0")
        return value

class User(BaseModel):
    id : int
    name : str
    email : EmailStr  # EmailStr magában validál

    @field_validator("id")
    def validate_id(cls, value):
        if value < 0:
            raise ValueError("Value must be bigger than 0")
        return value

class Basket(BaseModel):
    id : int
    user_id : int
    items : list[Item]

    @field_validator("id", "user_id")
    def validate_non_negative(cls, value):
        if value < 0:
            raise ValueError("Value must be bigger than 0")
        return value

