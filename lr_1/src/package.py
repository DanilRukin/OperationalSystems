import time
import uuid

class Package:
    """
    Package - пакет заданий для выполнения в системе
    """
    def __init__(self):
        self.__instructions = []
        self.__status = PackageStatus.New
        self.__current_time = 0
        self.__id = uuid.uuid4()

    def add_instruction(self, instruction):
        self.__instructions.append(instruction)

    def remove_instruction(self, instruction):
        self.__instructions.remove(instruction)

    @property
    def id(self):
        return self.__id

    @property
    def status(self):
        return self.__status

    @property
    def instruction(self):
        return self.__instructions


    def execute(self):
        if self.__status == PackageStatus.New:
            self.__start()
        elif self.__status == PackageStatus.Interrupted:
            self.__continue()
        else:
            raise Exception(f"Невозможно выполнить пакет {self.__id} со статусом {self.__status}")
        


    def __start(self):
        print(f"Начинает выполняться пакет {self.__id}")
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
                    return
                current_instruction.execute()
                self.__current_time = time.time() - timing
        else:
            print(f"Пакет {self.__id} не был выполнен, т.к. не было инструкций")
            return
        print(f"Пакет {self.__id} выполнен полностью")
        self.__status = PackageStatus.Completed
        return

    def __continue(self):
        print(f"Продолжает выполняться пакет {self.__id}")
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
                    self.__current_time += local_time
                    return
                current_instruction.execute()
                local_time = time.time() - timing
        else:
            print(f"Пакет {self.__id} не продолжил выполняться, т.к. не было инструкций")
            return
        self.__current_time += local_time
        print(f"Пакет {self.__id} выполнен полностью")
        self.__status = PackageStatus.Completed
        return



    




from enum import Enum
class PackageStatus(Enum):
    Executing = 1
    Interrupted = 2
    New = 3
    Completed = 4


