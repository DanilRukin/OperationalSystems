from my_process import *
import uuid
from datetime import datetime
from instruction import InstructionType


class TimeSharingSystem:
    def __init__(self, logger) -> None:
        self.logger = logger
        self.__id = uuid.uuid4()
        self.processes = []
        self.__current_time = 0

    def add_task(self, process):
        self.processes.append(process)

    def remove_task(self, process):
        self.processes.remove(process)


    def configure(self, config_function):
        config_function(self)

    @property
    def last_session_work_time(self):
        return self.__current_time


    def start(self, max_working_time, quantum_of_time):
        self.__current_time = 0
        self.logger.info(f'Система запущена в: {datetime.now()}')
        self.logger.info('Обработка процессов...')
        timing = time.time()
        working_time_of_all_executed_instructions = 0
        amount_of_executed_instructions = 0
        amount_of_executed_tasks = 0
        current_time = 0
        execution_time = 0
        while current_time < max_working_time and self.processes:
            process = self.processes.pop(0)
            process.execute(quantum_of_time)
            working_time_of_all_executed_instructions += process.last_session_execution_time
            amount_of_executed_instructions += process.last_session_instructions_executed
            if (process.status == PackageStatus.Interrupted):
                self.processes.append(process)
            elif process.status == PackageStatus.Completed:
                amount_of_executed_tasks += 1
                execution_time += process.current_working_time
            current_time = time.time() - timing
        if current_time > max_working_time:
            self.logger.info(f'Выполнение процессов прервано, т.к. превышен лимит времени: {max_working_time} c')
        else:
            self.logger.info('Все процессы обработаны')
        self.logger.info(f'Система остановлена в: {datetime.now()}')
        self.__current_time = current_time

         # время простоя процессора
        system_time = current_time if current_time > max_working_time else max_working_time        
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
   

    def get_count_of_IO_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.IO else False)
    
    def get_count_of_Process_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.Process else False)
    
    def calculate_instructions_count_where(self, predicate):
        count = 0
        for process in self.processes:
            for inst in process.instructions:
                if predicate(inst) == True:
                    count += 1
        return count
