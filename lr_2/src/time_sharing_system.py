from my_process import *
import uuid
from datetime import datetime
from instruction import InstructionType


class TimeSharingSystem:
    def __init__(self, logger) -> None:
        self.__logger = logger
        self.__id = uuid.uuid4()
        self.processes = []


    def configure(self, config_function):
        config_function(self)


    def start(self, max_working_time, quantum_of_time):
        self.__logger.info(f'Система запущена в: {datetime.now()}')
        self.__configure()
        self.__logger.info('Обработка процессов...')
        timing = time.time()
        current_time = 0
        while current_time < max_working_time and self.processes:
            process = self.processes.pop(0)
            process.execute(quantum_of_time)
            if (process.status == PackageStatus.Interrupted):
                self.processes.append(process)
            current_time = time.time() - timing
        if current_time > max_working_time:
            self.__logger.info(f'Выполнение процессов прервано, т.к. превышен лимит времени: {max_working_time} c')
        else:
            self.__logger.info('Все процессы обработаны')
        self.__logger.info(f'Система остановлена в: {datetime.now()}')    

    def get_count_of_IO_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.IO else False)
    
    def get_count_of_Process_instructions(self):
        return self.calculate_instructions_count_where(lambda inst: True if inst.instructionType == InstructionType.Process else False)
