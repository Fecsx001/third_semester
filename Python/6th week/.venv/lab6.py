from fastapi import FastAPI, HTTPException, Request
from typing import Optional
import uvicorn
from pydantic import BaseModel

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Welcome to the API"}

@app.get("welcome")
def welcome(name: Optional[str] = None, password: Optional[str] = None):
    if name and password == "password":
        return {"message": f"Welcome {name}!"}
    else:
        return {"message": f"Welcome guest!"}


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port= 8000)