import time
import uuid


class Instruction:
    def __init__(self, instruction_type, duration, logger):
        self.__duration = duration
        self.__instructionType = instruction_type
        self.__id = uuid.uuid4()
        self.__logger = logger

    @property
    def duration(self):
        return self.__duration
    
    @duration.setter
    def duration(self, duration):
        if duration < 0:
            message = "Продолжительность выполнения инструкции не может быть меньше нуля"
            self._logger.error(message)
            raise Exception(message)
        self.__duration = duration

    @property
    def instructionType(self):
        return self.__instructionType

    def execute(self):
        timing = time.time()
        current_time = 0
        message = f"Выполняется инструкция ({self.__id}) с типом {self.__instructionType}."
        self.__logger.info(message)
        while current_time < self.__duration:                       
            current_time = time.time() - timing
        self.__logger.info(f"Инструкция ({self.__id}) с типом {self.__instructionType} завершена. Время: {current_time}")
            


from enum import Enum

class InstructionType(Enum):
    Process = 1
    IO = 2

    def __str__(self):
        return str(self.name)