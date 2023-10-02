from time import process_time

class Package:
    """
    Package - пакет заданий для выполнения в системе
    """
    def __init__(self):
        self.__instructions = ''
        self.__status = PackageStatus.New
        self.__current_time = 0

    @property
    def status(self):
        return self.__status

    @property
    def instruction(self):
        return self.__instructions
    
    @instruction.setter
    def instructions(self, instructions):
        self.__instructions = instructions

    def start(self):
        self.__status = PackageStatus.Executing
        self.__current_time = process_time()

    




from enum import Enum
class PackageStatus(Enum):
    Executing = 1
    Interrupted = 2
    New = 3
    Closed = 4


