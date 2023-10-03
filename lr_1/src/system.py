import time
from package import *
import datetime

class System:
    """
    System - класс, представляющий собой операционную систему, функционирующую в
    пакетном режиме.
    """
    def __init__(self):
        self.__tasks = []
        self.__current_time = 0

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
        print(f"Система запущена в {now.strftime('%d-%m-%d %H:%M:%S')}")
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
        print(f"Система остановлена в {now.strftime('%d-%m-%d %H:%M:%S')}")
        return



