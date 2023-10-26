import time
import uuid

class Package:
    """
    Package - пакет заданий для выполнения в системе
    """
    def __init__(self, logger, queue):
        self.__instructions = []
        self.__status = PackageStatus.New
        self.__current_time = 0
        self.__id = uuid.uuid4()
        self.__logger = logger
        self.__instructions_executed = 0
        self.__last_session_instructions_executed = 0
        self.__last_session_execution_time = 0


    def add_instruction(self, instruction):
        self.__instructions.append(instruction)

    def remove_instruction(self, instruction):
        self.__instructions.remove(instruction)

    @property
    def last_session_execution_time(self):
        return self.__last_session_execution_time

    @property
    def last_session_instructions_executed(self):
        return self.__last_session_instructions_executed

    @property
    def current_working_time(self):
        return self.__current_time

    @property
    def instructions_executed(self):
        return self.__instructions_executed

    @property
    def id(self):
        return self.__id

    @property
    def status(self):
        return self.__status

    @property
    def instructions(self):
        return self.__instructions
        

    def execute(self, quant):
        if self.__status == PackageStatus.New:
            self.__logger.info(f"Выполняется пакет {self.__id}")
        elif self.__status == PackageStatus.Interrupted:
            self.__logger.info(f"Продолжает выполняться пакет {self.__id}")
        else:
            message = f"Невозможно выполнить пакет {self.__id} со статусом {self.__status}"
            self.__logger.error(message)
            raise Exception(message)
        self.__status = PackageStatus.Executing
        last_session_executed_tasks_count = 0
        self.__last_session_execution_time = 0
        self.__last_session_instructions_executed = 0
        local_time = 0        
        timing = time.time()       
        while local_time < quant and self.__instructions: # исполняемся в пределах кванта времени
            instruction = self.__instructions.pop(0)
            instruction.execute()
            self.__instructions_executed += 1
            last_session_executed_tasks_count += 1
            local_time = time.time() - timing
        if self.__instructions: # если истек квант времени
            self.__status = PackageStatus.Interrupted
            self.__logger.info(f"Пакет {self.__id} прервал выполнение")
        else: # закончились инструкции
            self.__status = PackageStatus.Completed
            self.__logger.info(f"Пакет {self.__id} выполнен полностью")      
        self.__current_time += local_time
        self.__last_session_execution_time = local_time
        self.__last_session_instructions_executed = last_session_executed_tasks_count
        return



from enum import Enum
class PackageStatus(Enum):
    Executing = 1
    Interrupted = 2
    New = 3
    Completed = 4


