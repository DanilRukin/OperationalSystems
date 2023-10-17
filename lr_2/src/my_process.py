import time
import uuid

class Package:
    """
    Package - пакет заданий для выполнения в системе
    """
    def __init__(self, logger):
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


    def execute(self):
        if self.__status == PackageStatus.New:
            self.__start()
        elif self.__status == PackageStatus.Interrupted:
            self.__continue()
        else:
            message = f"Невозможно выполнить пакет {self.__id} со статусом {self.__status}"
            self.__logger.error(message)
            raise Exception(message)
        


    def __start(self):
        last_session_executed_tasks_count = 0
        self.__last_session_execution_time = 0
        self.__logger.info(f"Начинает выполняться пакет {self.__id}")
        self.__status = PackageStatus.Executing
        self.__current_time = 0
        timing = time.time()
        if self.__instructions:
            previous_instruction_type = self.__instructions[0].instructionType
        
            while self.__instructions:
                current_instruction = self.__instructions.pop(0)
                # если типы инструкций не совпадают, то необходимо прервать выполнение пакета и передать управление системе
                if previous_instruction_type != current_instruction.instructionType:
                    # возвращаем инструкцию назад
                    self.__instructions.insert(0, current_instruction)
                    self.__status = PackageStatus.Interrupted
                    self.__current_time = time.time() - timing
                    self.__last_session_execution_time = self.__current_time
                    self.__logger.info(f"Пакет {self.__id} прервал выполнение")
                    self.__last_session_instructions_executed = last_session_executed_tasks_count
                    return
                current_instruction.execute()
                last_session_executed_tasks_count += 1
                self.__instructions_executed += 1
                self.__current_time = time.time() - timing
                self.__last_session_execution_time = self.__current_time
        else:
            self.__logger.info(f"Пакет {self.__id} не был выполнен, т.к. не было инструкций")
            self.__last_session_instructions_executed = last_session_executed_tasks_count
            return
        self.__logger.info(f"Пакет {self.__id} выполнен полностью")
        self.__status = PackageStatus.Completed
        self.__last_session_instructions_executed = last_session_executed_tasks_count
        return

    def __continue(self):
        last_session_executed_tasks_count = 0
        self.__last_session_execution_time = 0
        self.__logger.info(f"Продолжает выполняться пакет {self.__id}")
        self.__status = PackageStatus.Executing
        timing = time.time()
        local_time = 0
        if self.__instructions:
            previous_instruction_type = self.__instructions[0].instructionType
        
            while self.__instructions:
                current_instruction = self.__instructions.pop(0)
                # если типы инструкций не совпадают, то необходимо прервать выполнение пакета и передать управление системе
                if previous_instruction_type != current_instruction.instructionType:
                    # возвращаем инструкцию назад
                    self.__instructions.insert(0, current_instruction)
                    self.__status = PackageStatus.Interrupted
                    local_time = time.time() - timing
                    self.__last_session_execution_time = local_time
                    self.__current_time += local_time
                    self.__logger.info(f"Пакет {self.__id} прервал выполнение")
                    self.__last_session_instructions_executed = last_session_executed_tasks_count
                    return
                current_instruction.execute()
                last_session_executed_tasks_count += 1
                self.__instructions_executed += 1
                local_time = time.time() - timing
        else:
            self.__logger.info(f"Пакет {self.__id} не продолжил выполняться, т.к. не было инструкций")
            self.__last_session_instructions_executed = last_session_executed_tasks_count
            return
        self.__last_session_execution_time = local_time
        self.__current_time += local_time
        self.__logger.info(f"Пакет {self.__id} выполнен полностью")
        self.__status = PackageStatus.Completed
        self.__last_session_instructions_executed = last_session_executed_tasks_count
        return




from enum import Enum
class PackageStatus(Enum):
    Executing = 1
    Interrupted = 2
    New = 3
    Completed = 4


