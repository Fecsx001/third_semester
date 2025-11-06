import logging

from schemas.schema import User
from schemas.schema import Basket
from schemas.schema import Item

from utils.logger import log_io

from fastapi.responses import JSONResponse
from fastapi import HTTPException
from fastapi import APIRouter
from data.filehandler import add_user, add_basket, add_item_to_basket, save_json
from data.filereader import (
    get_all_users,
    get_total_price_of_basket,
    get_basket_by_user_id,
    get_user_by_id,
    load_json,
)

logger = logging.getLogger(__name__)

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
@log_io
def adduser(user: User) -> User:
    try:
        get_user_by_id(user.id)
        raise HTTPException(status_code=400, detail="User ID already in use")
    except ValueError:
        add_user(user.model_dump())
        return JSONResponse(content=user.model_dump(), status_code=200)

    """With the prewriten template this is the easiest solution, but not the best practice, due to the error msg-s in the log"""


@routers.post("/addshoppingbag")
@log_io
def addshoppingbag(userid: int) -> str:
    try:
        get_user_by_id(userid)
        data = load_json()

        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                raise HTTPException(
                    status_code=400,
                    detail=f"A shopping bag already exists for this user at id: {userid}",
                )

        new_basket = {
            "id": (
                max(basket["id"] for basket in data["Baskets"])
                if data["Baskets"]
                else 0 + 1
            ),
            "user_id": userid,
            "items": [],
        }
        add_basket(new_basket)
        return JSONResponse(
            content=f"Shopping bag created for user id: {userid} with basket id: {new_basket["id"]}",
            status_code=200,
        )
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.post("/additem", response_model=Basket)
@log_io
def additem(userid: int, item: Item) -> Basket:
    try:
        basket = get_basket_by_user_id(userid)
        add_item_to_basket(userid, item.model_dump())
        basket["items"].append(item.model_dump())
        return JSONResponse(
            content={"user_id": userid, "items": basket}, status_code=200
        )
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@routers.put("/updateitem")
@log_io
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
                raise HTTPException(
                    status_code=404,
                    detail=f"Item: {itemid}  not found in basket: {basket}",
                )
        raise HTTPException(
            status_code=404, detail=f"No basket found for user: {userid}"
        )
    except HTTPException as e:
        logger.error(str(e))
        raise e
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@routers.delete("/deleteitem")
@log_io
def deleteitem(userid: int, itemid: int) -> Basket:
    try:
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                for item in basket["items"]:
                    if item["item_id"] == itemid:
                        break
                else:
                    raise HTTPException(
                        status_code=404,
                        detail=f"Item id: {itemid} not found in the basket of user id: {userid}",
                    )
                basket["items"].remove(item)
                save_json(data)
                return JSONResponse(content=basket, status_code=200)
        raise HTTPException(
            status_code=404, detail=f"Basket not found for user id: {userid}"
        )
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@routers.get("/user", response_model=User)
@log_io
def user(userid: int) -> User:
    try:
        user = get_user_by_id(userid)
        return JSONResponse(content=user, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@routers.get("/users", response_model=list[User])
@log_io
def users() -> list[User]:
    users = get_all_users()
    return JSONResponse(content=users, status_code=200)


@routers.get("/shoppingbag", response_model=list[Item])
@log_io
def shoppingbag(userid: int) -> list[Item]:
    try:
        items = get_basket_by_user_id(userid)
        return JSONResponse(content=items, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@routers.get("/getusertotal")
@log_io
def getusertotal(userid: int) -> float:
    try:
        total = get_total_price_of_basket(userid)
        return JSONResponse(content={"total": total}, status_code=200)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))
