from abc import ABC, abstractmethod
from my_process import PackageStatus
import uuid

class ProcessQueue(ABC):
    def __init__(self, priority):
        self.__processes = []
        self.__id = uuid.uuid4()
        self.__priority = priority
        self.__can_execute = False

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
    def manage_processes(self):
        pass

    def stop(self):
        self.__can_execute = False



class RobinRoundQueue(ProcessQueue):

    def __init__(self, priority, quantum_of_time):
        super().__init__(priority)
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



    def manage_processes(self):
        self.__can_execute = True
        while self.__can_execute:
            if self.__processes:
                process = self.__processes.pop(0)
                # важно - общее время выполнения процесса не должно превышать квант времени
                process.execute()
                if process.status == PackageStatus.Interrupted:
                    if process.remaining_time() <= self.__quantum_of_time // 2:
                        # пытаемся переместить процесс в более приоритетную очередь
                        pass
                    else:
                        self.__processes.append(process)