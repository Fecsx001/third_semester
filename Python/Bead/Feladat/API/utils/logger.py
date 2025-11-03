import os
import logging
from functools import wraps
from logging.handlers import RotatingFileHandler
import inspect


def initialize_log(log_file: str, log_version: str, log_level=logging.DEBUG) -> None:
    """
    Initializes logging for the application with a rotating file handler.

    Parameters
    ----------
    log_file : str
        Name of the log file.
    log_version : str
        Version identifier for the log (not used in function, for reference).
    log_level : int, optional
        Logging level (default is logging.DEBUG).
    """
    log_dir = os.path.join(os.getcwd(), "output")
    os.makedirs(log_dir, exist_ok=True)
    log_file_path = os.path.join(log_dir, log_file)

    # Create rotating file handler
    rotating_handler = RotatingFileHandler(
        log_file_path, maxBytes=2 * 1024 * 1024, backupCount=4, encoding="utf-8"
    )
    rotating_handler.setLevel(log_level)
    rotating_handler.setFormatter(
        logging.Formatter(
            "[%(levelname).1s] [%(asctime)s] [%(name)s:%(lineno)d] %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
        )
    )

    # Configure root logger
    root_logger = logging.getLogger()
    root_logger.setLevel(log_level)
    root_logger.handlers.clear()
    root_logger.addHandler(rotating_handler)

    # Configure uvicorn loggers
    for name in ["uvicorn", "uvicorn.error", "uvicorn.access", "fastapi"]:
        uv_logger = logging.getLogger(name)
        uv_logger.setLevel(log_level)
        uv_logger.handlers.clear()
        uv_logger.addHandler(rotating_handler)
        uv_logger.propagate = False

    # Optional: disable console output entirely
    logging.getLogger().propagate = False

    root_logger.info(f"Logging initialized -> {log_file_path}")


def log_io(func):
    """Decorator for logging function input/output."""

    logger_name = func.__module__
    logger = logging.getLogger(logger_name)

    @wraps(func)
    def wrapper(*args, **kwargs):
        try:
            logger.info(f"Started running {func.__name__}")
            signature = inspect.signature(func)
            params = signature.parameters
            if args or kwargs:
                param_list = []
                for i, (name, param) in enumerate(params.items()):
                    if i < len(args):
                        if name == "self":
                            continue
                        if name == "cls" and i == 0:
                            continue
                        param_list.append(f"{name}={args[i]}")

                for name, value in kwargs.items():
                    param_list.append(f"{name}={value}")

                if param_list:
                    logger.debug(f"Arguments: {', '.join(param_list)}")
            else:
                logger.debug(f"No arguments provided to {func.__name__}")

            result = func(*args, **kwargs)
            logger.info(f"Finished running {func.__name__}")
            if result is not None:
                if isinstance(result, (dict, list)) and len(str(result)) > 100:
                    logger.debug(
                        f"Return: {type(result).__name__} (size: {len(str(result))} chars)"
                    )
                else:
                    logger.debug(f"Return: {result}")
            return result
        except Exception as e:
            logger.exception(f"Error in {func.__name__}: {e}")
            raise

    return wrapper
