import time
from package import *
import datetime
from operator import attrgetter
from instruction import InstructionType


class System:
    """
    System - класс, представляющий собой операционную систему, функционирующую в
    пакетном режиме.
    """
    def __init__(self, logger):
        self.__tasks = []
        self.__current_time = 0
        self.__logger = logger

    @property
    def logger(self):
        return self.__logger

    @property
    def tasks(self):
        return self.__tasks
    
    @property
    def last_session_work_time(self):
        return self.__current_time
    
    def calculate_instructions_count_where(self, predicate):
        count = 0
        for task in self.__tasks:
            for inst in task.instructions:
                if predicate(inst) == True:
                    count += 1
        return count
    

    def get_count_of_IO_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.IO else False)
    
    def get_count_of_Process_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.Process else False)
        

    def configure(self, config_function):
        config_function(self)

    def add_task(self, package):
        self.__tasks.append(package)

    def remove_task(self, package):
        self.__tasks.remove(package)

    def count_and_time_of_all_instructions_could_be_executed_by_system(self, max_execution_time):
        instructions = []
        for task in self.__tasks:
            instructions.extend(task.instructions)
        instructions.sort(key=lambda inst: inst.duration)
        count = 0
        common_time = 0
        for inst in instructions:
            if (common_time + inst.duration) > max_execution_time:
                break
            else:
                common_time += inst.duration
                count += 1
        return count, common_time

    def compare_instructions(instruction_a, instruction_b):
        return attrgetter('__duration')(instruction_a) - attrgetter('__duration')(instruction_b)

    def start(self, max_execution_time):
        self.__current_time = 0
        amount_of_executed_instructions = 0
        amount_of_executed_tasks = 0
        working_time_of_all_executed_instructions = 0
        execution_time = 0
        productivity = 0.0 # производительность
        working_time = 0 # оборотное время
        processor_downtime = 0 # время простоя процессора
        now = datetime.datetime.now()
        self.__logger.info(f"Система запущена в {now.strftime('%d-%m-%d %H:%M:%S')}")
        timing = time.time()
        # пока в очереди задач еще что-то есть
        while self.__tasks and self.__current_time < max_execution_time:
            current_task = self.__tasks.pop(0)
            current_task.execute()
            amount_of_executed_instructions += current_task.last_session_instructions_executed
            working_time_of_all_executed_instructions += current_task.last_session_execution_time
            # если задача отдала управление, но не закончилась
            if current_task.status == PackageStatus.Interrupted:
                self.__tasks.append(current_task)
            elif current_task.status == PackageStatus.Completed:
                amount_of_executed_tasks += 1
                execution_time += current_task.current_working_time
            self.__current_time = time.time() - timing
        now = datetime.datetime.now()
        self.__logger.info(f"Система остановлена в {now.strftime('%d-%m-%d %H:%M:%S')}")
        
        # время простоя процессора
        system_time = self.__current_time if self.__current_time > max_execution_time else max_execution_time        
        processor_downtime = system_time - working_time_of_all_executed_instructions
        if processor_downtime < 0:
            processor_downtime = 0
        elif processor_downtime > system_time:
            processor_downtime = system_time

        # производительность            
        productivity = amount_of_executed_instructions / system_time

        # оборотное время
        if amount_of_executed_tasks == 0:
            working_time = 0
        else:
            working_time = execution_time / amount_of_executed_tasks

        return productivity, working_time, processor_downtime
