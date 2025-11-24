from schemas.schema import User, Basket, Item
from fastapi.responses import JSONResponse, RedirectResponse
from fastapi import FastAPI, HTTPException, Request, Response, Cookie
from fastapi import APIRouter
from data.filereader import *
from data.filehandler import *

"""

Útmutató a fájl használatához:

- Minden route esetén adjuk meg a response_modell értékét (típus)
- Ügyeljünk a típusok megadására
- A függvények visszatérési értéke JSONResponse() legyen
- Minden függvény tartalmazzon hibakezelést, hiba esetén dobjon egy HTTPException-t
- Az adatokat a data.json fájlba kell menteni.
- A HTTP válaszok minden esetben tartalmazzák a 
  megfelelő Státus Code-ot, pl 404 - Not found, vagy 200 - OK

"""

routers = APIRouter()


@routers.post("/adduser", response_model=User)
def adduser(user: User) -> User:
    try:
        get_user_by_id(user.id)
        raise HTTPException(status_code=400, detail="User already exists")
    except ValueError:
        add_user(user.model_dump())
        return JSONResponse(content=user.model_dump(), status_code=201)
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@routers.post("/addshoppingbag")
def addshoppingbag(userid: int) -> str:
    try:
        get_user_by_id(userid)
        temp = load_json()
        if any(basket["user_id"] == userid for basket in temp["Baskets"]):
            raise HTTPException(status_code=400, detail="Basket already exists")
        new_basket = {
            "id": max(basket["id"] for basket in temp["Baskets"]) + 1,
            "user_id": userid,
            "items": [],
        }
        add_basket(new_basket)
        return JSONResponse(content="Basket created successfully", status_code=201)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.post("/additem", response_model=Basket)
def additem(userid: int, item: Item) -> Basket:
    try:
        basket_items = get_basket_by_user_id(userid)
        add_item_to_basket(userid, item.model_dump())
        basket_items.append(item.model_dump())
        return JSONResponse(
            content={"user_id": userid, "items": basket_items}, status_code=200
        )
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.put("/updateitem")
def updateitem(userid: int, itemid: int, updateItem: Item) -> Basket:
    try:
        temp = load_json()
        for basket in temp["Baskets"]:
            if basket["user_id"] == userid:
                for item in basket["items"]:
                    if item["item_id"] == itemid:
                        item.update(updateItem.model_dump())
                        save_json(temp)
                        return JSONResponse(content=basket, status_code=200)
                raise HTTPException(status_code=404, detail="Item not found")
        raise HTTPException(status_code=404, detail="Basket not found")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.delete("/deleteitem")
def deleteitem(userid: int, itemid: int) -> Basket:
    try:
        temp = load_json()
        for basket in temp["Baskets"]:
            if basket["user_id"] == userid:
                item = next(
                    (i for i in basket["items"] if i["item_id"] == itemid), None
                )
                if not item:
                    raise HTTPException(status_code=404, detail="Item not found")
                basket["items"].remove(item)
                save_json(temp)
                return JSONResponse(content=basket, status_code=200)
        raise HTTPException(status_code=404, detail="Basket not found")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/user")
def user(userid: int) -> User:
    try:
        user = get_user_by_id(userid)
        return JSONResponse(content=user, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/users")
def users() -> list[User]:
    users = get_all_users()
    return JSONResponse(content=users, status_code=200)


@routers.get("/shoppingbag")
def shoppingbag(userid: int) -> list[Item]:
    try:
        items = get_basket_by_user_id(userid)
        return JSONResponse(content=items, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/getusertotal")
def getusertotal(userid: int) -> float:
    try:
        price_sum = get_total_price_of_basket(userid)
        return JSONResponse(content={"total": price_sum}, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
