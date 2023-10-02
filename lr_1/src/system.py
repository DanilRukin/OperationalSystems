from time import process_time

class System:
    """
    System - класс, представляющий собой операционную систему, функционирующую в
    пакетном режиме.
    """
    def __init__(self, tasks):
        self.__tasks = tasks
        self.__current_time_ms = 0

    def add_task(self, package):
        self.__tasks.append(package)

    def remove_task(self, package):
        self.__tasks.remove(package)

    def start(self):
        self.__current_time_ms = process_time()

    def stop(self):
        self.__current_time_ms = process_time() - self.__current_time_ms

    def get_current_time_ms(self):
        return self.__current_time_ms
    
    def get_current_time_s(self):
        return self.__current_time_ms / 1000
    
    def get_current_time_min(self):
        return self.get_current_time_s() / 60
    
    def get_current_time_hour(self):
        return self.get_current_time_min() / 60
