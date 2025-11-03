from schemas.schema import User, Basket, Item
from fastapi.responses import JSONResponse
from fastapi import HTTPException, APIRouter
from data.filehandler import add_user, add_basket, add_item_to_basket, save_json
from data.filereader import (
    get_user_by_id,
    get_basket_by_user_id,
    get_all_users,
    get_total_price_of_basket,
    load_json,
)

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
        # Ellenőrizzük, hogy a felhasználó már létezik-e
        get_user_by_id(user.id)
        raise HTTPException(status_code=400, detail="User ID already exists")
    except ValueError:
        add_user(user.model_dump())
        return JSONResponse(content=user.model_dump(), status_code=201)


@routers.post("/addshoppingbag")
def addshoppingbag(userid: int) -> str:
    try:
        # Ellenőrizzük, hogy a felhasználó létezik-e
        get_user_by_id(userid)
        data = load_json()
        if any(basket["user_id"] == userid for basket in data["Baskets"]):
            raise HTTPException(
                status_code=400, detail="Shopping bag already exists for this user"
            )
        # Új kosár hozzáadása
        new_basket = {
            "id": max(basket["id"] for basket in data["Baskets"]) + 1,
            "user_id": userid,
            "items": [],
        }
        add_basket(new_basket)
        return JSONResponse(
            content="Shopping bag created successfully", status_code=201
        )
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.post("/additem", response_model=Basket)
def additem(userid: int, item: Item) -> Basket:
    try:
        # Ellenőrizzük, hogy a kosár létezik-e
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
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                for item in basket["items"]:
                    if item["item_id"] == itemid:
                        item.update(updateItem.model_dump())
                        save_json(data)
                        return JSONResponse(content=basket, status_code=200)
                raise HTTPException(status_code=404, detail="Item not found in basket")
        raise HTTPException(status_code=404, detail="Basket not found for this user")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.delete("/deleteitem")
def deleteitem(userid: int, itemid: int) -> Basket:
    try:
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                item = next(
                    (i for i in basket["items"] if i["item_id"] == itemid), None
                )
                if not item:
                    raise HTTPException(
                        status_code=404, detail="Item not found in basket"
                    )
                basket["items"].remove(item)
                save_json(data)
                return JSONResponse(content=basket, status_code=200)
        raise HTTPException(status_code=404, detail="Basket not found for this user")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/user", response_model=User)
def user(userid: int) -> User:
    try:
        user = get_user_by_id(userid)
        return JSONResponse(content=user, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/users", response_model=list[User])
def users() -> list[User]:
    users = get_all_users()
    return JSONResponse(content=users, status_code=200)


@routers.get("/shoppingbag", response_model=list[Item])
def shoppingbag(userid: int) -> list[Item]:
    try:
        items = get_basket_by_user_id(userid)
        return JSONResponse(content=items, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.get("/getusertotal")
def getusertotal(userid: int) -> float:
    try:
        total = get_total_price_of_basket(userid)
        return JSONResponse(content={"total": total}, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
