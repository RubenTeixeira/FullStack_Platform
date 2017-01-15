<?php
/**
 * Global Configuration Override
 *
 * You can use this file for overriding configuration values from modules, etc.
 * You would place values in here that are agnostic to the environment and not
 * sensitive to security.
 *
 * @NOTE: In practice, this file will typically be INCLUDED in your source
 * control, so do not include passwords or other sensitive information in this
 * file.
 */
return array(
    // ...
    
    'navigation' => array(
        'default' => array(
            
            // 'pages' => array(
            // array(
            // 'label' => 'Child #1',
            // 'route' => 'page-1-child',
            // ),
            // ),
            array(
                'label' => 'Pontos de Interesse',
                'route' => 'poi'
            ),
        ),
        'authenticated' => array(
            array(
                'label' => 'Visitas',
                'route' => 'visita'
            ),
            array(
                'label' => 'Percursos',
                'route' => 'percurso'
            ),
            array(
                'label' => 'Transferências',
                'route' => 'download'
            )
        ),
        'session' => array(
            array(
                'label' => 'Perfil',
                'route' => 'user',
                'action' => 'info'
            ),
            array(
                'label' => 'Sair',
                'route' => 'user',
                'action' => 'logout'
            )
        ),
        'guest' => array(
            array(
                'label' => 'Entrar',
                'route' => 'user',
                'action' => 'login'
            ),
            array(
                'label' => 'Registar',
                'route' => 'user',
                'action' => 'register'
            )
        ),
    ),
    
    'service_manager' => array(
        'factories' => array(
            'navigation' => 'Zend\Navigation\Service\NavigationAbstractServiceFactory'
        )
    )
);
// ...
