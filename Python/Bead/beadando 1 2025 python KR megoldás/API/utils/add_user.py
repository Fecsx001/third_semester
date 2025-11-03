from API.schemas.schema import User
from API.data.filehandler import add_user


class AddUser:
    def __init__(self, user: User):
        new_user = new_user = {
            "id": user.id,
            "name": user.name,
            "email": user.email,
        }

        add_user(new_user)
