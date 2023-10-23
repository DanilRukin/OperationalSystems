from my_process import *
from queue import *
import uuid
from datetime import datetime


class MultilevelSystem:
    def __init__(self, logger) -> None:
        self.__logger = logger
        self.__id = uuid.uuid4()
        self.__queues = []
        self.__processes = []

    def add_queue(self, queue):
        self.__queues.append(queue)


    def remove_queue(self, queue):
        self.__queues.remove(queue)

    def start(self):
        self.__logger.info(f'System started at: {datetime.now()}')
        self.__configure()
        self.__logger.info('Starting process the processes...')
        for queue in self.__queues:
            queue.manage_processes(self)
        self.__logger.info('All processes have been processed')
        self.__logger.info(f'System stopped at: {datetime.now()}')

    
    def try_move_process_to_another_queue(self, process):
        process_was_moved = False
        for queue in self.__queues:
            if queue.could_add_process(process):
                queue.add_process(process)
                self.__logger.info(f'Process {process} moved to queue {queue}')
                process_was_moved = True
                break
        return process_was_moved
    
    def are_another_more_priority_queues_have_processes(self, current_queue):
        for queue in self.__queues:
            if current_queue != queue:
                if queue.has_processes():
                    return True
            else:
                return False


    def __configure(self):
        self.__logger.info(f'Starting configuring...')
        # сортируем от меньшего к большему (чем ниже цифра, тем выше приоритет, самый высокий приоритет = 0)
        self.__logger.info(f'Sorting queues...')
        self.__queues = sorted(self.__queues, key=lambda queue: queue.priority) 
        self.__logger.info(f'Sorted queues:\r\n')
        for queue in self.__queues:
            self.__logger.info(f'\t{queue} with priority: {queue.priority}')
        # пытаемся засунуть процесс в самую приоритетную очередь
        process_was_added = False
        while self.__processes:
            process_was_added = False
            process = self.__processes.pop(0)
            for queue in self.__queues:
                if queue.could_add_process(process):
                    queue.add_process(process)
                    self.__logger.info(f'Process {process} added to queue {queue}')
                    process_was_added = True
                    break
            if not process_was_added:
                raise Exception(f'Process {process} was not added to any queue!!!')
        self.__logger.info('All processes were distributed')
        self.__logger.info(f'Configuring completed')
    
