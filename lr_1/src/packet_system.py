import time
from package import *
import datetime
import logging
from logging.handlers import TimedRotatingFileHandler
import os


class System:
    """
    System - класс, представляющий собой операционную систему, функционирующую в
    пакетном режиме.
    """
    def __init__(self):
        self.__tasks = []
        self.__current_time = 0

        # определяем формат сообщений
        formatter = logging.Formatter('%(asctime)s %(name)s %(message)s')
        log_file = 'myapp.log'
        if not os.path.exists('logs'):
            os.mkdir('logs')
        log_dir = 'logs'
        logfile = log_dir + '/' + log_file

        # создадим файл логов и обработчик
        fh = TimedRotatingFileHandler(logfile, when='midnight')
        fh.setFormatter(formatter)

        # установим уровень логирования на DEBUG для всех модулей
        ch = logging.StreamHandler()
        ch.setLevel(logging.DEBUG)
        ch.setFormatter(formatter)

        console_level = logging.DEBUG
        file_level = logging.DEBUG

        logger = logging.getLogger()
        logger.setLevel(console_level)
        logger.addHandler(fh)
        logger.addHandler(ch)

        self.__logger = logger

    @property
    def logger(self):
        return self.__logger

    @property
    def tasks(self):
        return self.__tasks
    
    @property
    def working_time(self):
        return self.__current_time

    def configure(self, config_function):
        config_function(self)

    def add_task(self, package):
        self.__tasks.append(package)

    def remove_task(self, package):
        self.__tasks.remove(package)

    def start(self, max_execution_time):
        now = datetime.datetime.now()
        self.__logger.info(f"Система запущена в {now.strftime('%d-%m-%d %H:%M:%S')}")
        timing = time.time()
        # пока в очереди задач еще что-то есть
        while self.__tasks and self.__current_time < max_execution_time:
            current_task = self.__tasks.pop(0)
            current_task.execute()
            # если задача отдала управление, но не закончилась
            if current_task.status == PackageStatus.Interrupted:
                self.__tasks.append(current_task)
            self.__current_time = time.time() - timing
        now = datetime.datetime.now()
        self.__logger.info(f"Система остановлена в {now.strftime('%d-%m-%d %H:%M:%S')}")
        return



