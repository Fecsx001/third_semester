import os
import logging
from functools import wraps
from logging.handlers import RotatingFileHandler
import inspect


def initialize_log(log_file: str, log_version: str, log_level=logging.INFO) -> None:
    """
    Initializes logging for the application with a rotating file handler.

    Parameters
    ----------
    log_file : str
        Name of the log file.
    log_version : str
        Version identifier for the log (not used in function, for reference).
    log_level : int, optional
        Logging level (default is logging.INFO).
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
            "[%(levelname).1s] [%(asctime)s] [%(filename)s:%(lineno)d] %(message)s",
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

    @wraps(func)
    def wrapper(*args, **kwargs):
        logger = logging.getLogger(func.__module__)
        try:
            logger.info(f"Started running {func.__name__}")
            signature = inspect.signature(func)
            params = signature.parameters

            # Log arguments
            if args or kwargs:
                param_list = []
                for i, (name, _) in enumerate(params.items()):
                    if i < len(args) and name != "self":
                        param_list.append(f"{name}={args[i]}")
                for name, value in kwargs.items():
                    param_list.append(f"{name}={value}")
                logger.debug(f"Arguments: {', '.join(param_list)}")
            else:
                logger.warning(f"No arguments provided to {func.__name__}")

            result = func(*args, **kwargs)
            logger.info(f"Finished running {func.__name__}")
            if result is not None:
                logger.debug(f"Return: {result}")
            return result
        except Exception as e:
            logger.exception(f"Error in {func.__name__}: {e}")
            raise

    return wrapper
