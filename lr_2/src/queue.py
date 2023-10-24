from abc import ABC, abstractmethod
from my_process import PackageStatus
import uuid
import events


class ProcessQueue(ABC):
    def __init__(self, priority, logger, sys):
        self.__processes = []
        self.__id = uuid.uuid4()
        self.__priority = priority
        self.__logger = logger
        self.__can_execute = False
        sys.stop_system_event.subscribe(self.system_stopped_handler)
        self.queue_interrupted_event = events.Event()

    @property
    def priority(self):
        return self.__priority
    
    @priority.setter
    def priority(self, priority):
        self.__priority = priority

    def add_process(self, process):
        self.__processes.append(process)

    def remove_process(self, process):
        self.__processes.remove(process)

    @abstractmethod
    def manage_processes(self, sys):
        pass

    @abstractmethod
    def could_add_process(self, process):
        pass

    def stop(self):
        self.__can_execute = False

    def has_processes(self):
        return True if self.__processes else False
    
    def system_stopped_handler(self, _, __):
        self.stop()
        self.__logger.info(f'{self} was stopped by system')
        



class RobinRoundQueue(ProcessQueue):

    def __init__(self, priority, logger, sys, quantum_of_time):
        super().__init__(priority, logger, sys)
        self.__quantum_of_time = quantum_of_time

    @property
    def quantum_of_time(self):
        return self.__quantum_of_time
    
    @quantum_of_time.setter
    def quantum_of_time(self, quantum_of_time):
        if quantum_of_time <= 0:
            raise Exception('Квант времени не может быть меньше, либо равен нулю')
        else:
            self.__quantum_of_time = quantum_of_time

    def __str__(self) -> str:
        return f'Очередь ({self.__id}): алгоритм - RobinRound'
    
    def could_add_process(self, process):
        return True if process.remaining_time() <= self.__quantum_of_time else False



    def manage_processes(self, sys):
        self.__can_execute = True
        while self.__can_execute and self.__processes:
            process = self.__processes.pop(0)
            # важно - общее время выполнения процесса не должно превышать квант времени
            process.start()
            if process.status == PackageStatus.Interrupted:
                if process.remaining_time() <= self.__quantum_of_time // 2:
                    # пытаемся переместить процесс в более приоритетную очередь
                    pass
                else:
                    self.__processes.append(process)
                



class FcfsQueue(ProcessQueue):
    def __init__(self, priority, logger, sys):
        super().__init__(priority, logger, sys)

    def manage_processes(self, sys):
        # Процессы выполняются друг за другом до тех пор, пока не выполнятся полностью
        self.__logger.info(f'FCFS ({self.__id}): начало выполнения набора процессов...')
        process
        while self.__can_execute and self.__processes:
            process = self.__processes.pop(0)
            process.start()
            if sys.are_another_more_priority_queues_have_processes(self):
                self.on_queue_interrupted()
                break
        pass

    def on_queue_interrupted(self):
        self.queue_interrupted_event.invoke(self, None)


    def __str__(self) -> str:
        return f'Очередь ({self.__id}): алгоритм - FCFS'
    
    def could_add_process(self, process):
        return True
