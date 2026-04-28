window.theme = {};

// Navigation
(function ($) {

    'use strict';

    var $items = $('.nav-main li.nav-parent');

    function expand($li) {
        $li.children('ul.nav-children').slideDown('fast', function () {
            $li.addClass('nav-expanded');
            $(this).css('display', '');
            ensureVisible($li);
        });
    }

    function collapse($li) {
        $li.children('ul.nav-children').slideUp('fast', function () {
            $(this).css('display', '');
            $li.removeClass('nav-expanded');
        });
    }

    function ensureVisible($li) {
        var scroller = $li.offsetParent();
        if (!scroller.get(0)) {
            return false;
        }

        var top = $li.position().top;
        if (top < 0) {
            scroller.animate({
                scrollTop: scroller.scrollTop() + top
            }, 'fast');
        }
    }

    $items.find('> a').on('click', function (ev) {

        var $anchor = $(this),
			$prev = $anchor.closest('ul.nav').find('> li.nav-expanded'),
			$next = $anchor.closest('li');

        if ($anchor.prop('href')) {
            var arrowWidth = parseInt(window.getComputedStyle($anchor.get(0), ':after').width, 10) || 0;
            if (ev.offsetX > $anchor.get(0).offsetWidth - arrowWidth) {
                ev.preventDefault();
            }
        }

        if ($prev.get(0) !== $next.get(0)) {
            collapse($prev);
            expand($next);
        } else {
            collapse($prev);
        }
    });


}).apply(this, [jQuery]);

// Skeleton
(function (theme, $) {

    'use strict';

    theme = theme || {};

    var $body = $('body'),
		$html = $('html'),
		$window = $(window),
		isAndroid = navigator.userAgent.toLowerCase().indexOf('android') > -1;

    // mobile devices with fixed has a lot of issues when focus inputs and others...
    if (typeof $.browser !== 'undefined' && $.browser.mobile && $html.hasClass('fixed')) {
        $html.removeClass('fixed').addClass('scroll');
    }

    var Skeleton = {

        options: {
            sidebars: {
                menu: '#content-menu',
                left: '#sidebar-left',
                right: '#sidebar-right'
            }
        },

        customScroll: (!Modernizr.overflowscrolling && !isAndroid && $.fn.nanoScroller !== 'undefined'),

        initialize: function () {
            this
				.setVars()
				.build()
				.events();
        },

        setVars: function () {
            this.sidebars = {};

            this.sidebars.left = {
                $el: $(this.options.sidebars.left)
            };

            this.sidebars.right = {
                $el: $(this.options.sidebars.right),
                isOpened: $html.hasClass('sidebar-right-opened')
            };

            this.sidebars.menu = {
                $el: $(this.options.sidebars.menu),
                isOpened: $html.hasClass('inner-menu-opened')
            };

            return this;
        },

        build: function () {

            if (typeof $.browser !== 'undefined' && $.browser.mobile) {
                $html.addClass('mobile-device');
            } else {
                $html.addClass('no-mobile-device');
            }

            $html.addClass('custom-scroll');
            if (this.customScroll) {
                this.buildSidebarLeft();
                this.buildContentMenu();
            }

            this.buildSidebarRight();

            return this;
        },

        events: function () {
            if (this.customScroll) {
                this.eventsSidebarLeft();
            }

            this.eventsSidebarRight();
            this.eventsContentMenu();

            if (typeof $.browser !== 'undefined' && !this.customScroll && isAndroid) {
                this.fixScroll();
            }

            return this;
        },

        fixScroll: function () {
            var _self = this;

            $window
				.on('sidebar-left-opened sidebar-right-toggle', function (e, data) {
				    _self.preventBodyScrollToggle(data.added);
				});
        },

        buildSidebarLeft: function () {
            this.sidebars.left.$nano = this.sidebars.left.$el.find('.nano');

            this.sidebars.left.$nano.nanoScroller({
                alwaysVisible: true,
                preventPageScrolling: true
            });

            return this;
        },

        eventsSidebarLeft: function () {

            var $nano = this.sidebars.left.$nano;

            var updateNanoScroll = function () {
                if ($.support.transition) {
                    $nano.nanoScroller();
                    $nano
						.one('bsTransitionEnd', updateNanoScroll)
						.emulateTransitionEnd(150)
                } else {
                    updateNanoScroll();
                }
            };

            this.sidebars.left.$el
				.on('click', function () {
				    updateNanoScroll();
				});

            $nano
				.on('mouseenter', function () {
				    if ($html.hasClass('sidebar-left-collapsed')) {
				        $nano.nanoScroller();
				    }
				})
				.on('mouseleave', function () {
				    if ($html.hasClass('sidebar-left-collapsed')) {
				        $nano.nanoScroller();
				    }
				});

            return this;
        },

        buildSidebarRight: function () {
            this.sidebars.right.isOpened = $html.hasClass('sidebar-right-opened');

            if (this.customScroll) {
                this.sidebars.right.$nano = this.sidebars.right.$el.find('.nano');

                this.sidebars.right.$nano.nanoScroller({
                    alwaysVisible: true,
                    preventPageScrolling: true
                });
            }

            return this;
        },

        eventsSidebarRight: function () {
            var _self = this;

            var open = function () {
                if (_self.sidebars.right.isOpened) {
                    return close();
                }

                _self.sidebars.right.isOpened = true;

                $html.addClass('sidebar-right-opened');

                $window.trigger('sidebar-right-toggle', {
                    added: true,
                    removed: false
                });

                $html.on('click.close-right-sidebar', function (e) {
                    e.stopPropagation();
                    close(e);
                });
            };

            var close = function (e) {
                if (!!e && !!e.target && ($(e.target).closest('.sidebar-right').get(0) || !$(e.target).closest('html').get(0))) {
                    e.preventDefault();
                    return false;
                }

                $html.removeClass('sidebar-right-opened');
                $html.off('click.close-right-sidebar');

                $window.trigger('sidebar-right-toggle', {
                    added: false,
                    removed: true
                });

                _self.sidebars.right.isOpened = false;
            };

            var bind = function () {
                $('[data-open="sidebar-right"]').on('click', function (e) {
                    var $el = $(this);
                    e.stopPropagation();

                    if ($el.is('a'))
                        e.preventDefault();

                    open();
                });
            };

            this.sidebars.right.$el.find('.mobile-close')
				.on('click', function (e) {
				    e.preventDefault();
				    $html.trigger('click.close-right-sidebar');
				});

            bind();

            return this;
        },

        buildContentMenu: function () {
            if (!$html.hasClass('fixed')) {
                return false;
            }

            this.sidebars.menu.$nano = this.sidebars.menu.$el.find('.nano');

            this.sidebars.menu.$nano.nanoScroller({
                alwaysVisible: true,
                preventPageScrolling: true
            });

            return this;
        },

        eventsContentMenu: function () {
            var _self = this;

            var open = function () {
                if (_self.sidebars.menu.isOpened) {
                    return close();
                }

                _self.sidebars.menu.isOpened = true;

                $html.addClass('inner-menu-opened');

                $window.trigger('inner-menu-toggle', {
                    added: true,
                    removed: false
                });

                $html.on('click.close-inner-menu', function (e) {

                    close(e);
                });

            };

            var close = function (e) {
                if (!!e && !!e.target && !$(e.target).closest('.inner-menu-collapse').get(0) && ($(e.target).closest('.inner-menu').get(0) || !$(e.target).closest('html').get(0))) {
                    return false;
                }

                e.stopPropagation();

                $html.removeClass('inner-menu-opened');
                $html.off('click.close-inner-menu');

                $window.trigger('inner-menu-toggle', {
                    added: false,
                    removed: true
                });

                _self.sidebars.menu.isOpened = false;
            };

            var bind = function () {
                $('[data-open="inner-menu"]').on('click', function (e) {
                    var $el = $(this);
                    e.stopPropagation();

                    if ($el.is('a'))
                        e.preventDefault();

                    open();
                });
            };

            bind();

            /* Nano Scroll */
            if ($html.hasClass('fixed')) {
                var $nano = this.sidebars.menu.$nano;

                var updateNanoScroll = function () {
                    if ($.support.transition) {
                        $nano.nanoScroller();
                        $nano
							.one('bsTransitionEnd', updateNanoScroll)
							.emulateTransitionEnd(150)
                    } else {
                        updateNanoScroll();
                    }
                };

                this.sidebars.menu.$el
					.on('click', function () {
					    updateNanoScroll();
					});
            }

            return this;
        },

        preventBodyScrollToggle: function (shouldPrevent, $el) {
            setTimeout(function () {
                if (shouldPrevent) {
                    $body
						.data('scrollTop', $body.get(0).scrollTop)
						.css({
						    position: 'fixed',

						    top: $body.get(0).scrollTop * -1
						})
                } else {
                    $body
						.css({
						    position: '',
						    top: ''
						})
						.scrollTop($body.data('scrollTop'));
                }
            }, 150);
        }

    };

    // expose to scope
    $.extend(theme, {
        Skeleton: Skeleton
    });

}).apply(this, [window.theme, jQuery]);

// Base
(function (theme, $) {

    'use strict';

    theme = theme || {};

    theme.Skeleton.initialize();

}).apply(this, [window.theme, jQuery]);

window.theme = {};


// Lock Screen 
(function ($) {

    'use strict';

    var LockScreen = {

        initialize: function () {
            this.$body = $('body');

            this
				.build()
				.events();
        },

        build: function () {
            var lockHTML,
				userinfo;

            userinfo = this.getUserInfo();
            this.lockHTML = this.buildTemplate(userinfo);

            this.$lock = this.$body.children('#LockScreenInline');
            this.$userPicture = this.$lock.find('#LockUserPicture');
            this.$userName = this.$lock.find('#LockUserName');
            this.$userEmail = this.$lock.find('#LockUserEmail');

            return this;
        },

        events: function () {
            var _self = this;

            this.$body.find('[data-lock-screen="true"]').on('click', function (e) {
                e.preventDefault();

                _self.show();
            });

            return this;
        },

        formEvents: function ($form) {
            var _self = this;

            $form.on('submit', function (e) {
                e.preventDefault();

                _self.hide();
            });
        },

        show: function () {
            var _self = this,
				userinfo = this.getUserInfo();

            this.$userPicture.attr('src', userinfo.picture);
            this.$userName.text(userinfo.username);
            this.$userEmail.text(userinfo.email);

            this.$body.addClass('show-lock-screen');

            $.magnificPopup.open({
                items: {
                    src: this.lockHTML,
                    type: 'inline'
                },
                modal: true,
                mainClass: 'mfp-lock-screen',
                callbacks: {
                    change: function () {
                        _self.formEvents(this.content.find('form'));
                    }
                }
            });
        },

        hide: function () {
            $.magnificPopup.close();
        },

        getUserInfo: function () {
            var $info,
				picture,
				name,
				email;

            // always search in case something is changed through ajax
            $info = $('#userbox');
            picture = $info.find('.profile-picture img').attr('data-lock-picture');
            name = $info.find('.profile-info').attr('data-lock-name');
            email = $info.find('.profile-info').attr('data-lock-email');

            return {
                picture: picture,
                username: name,
                email: email
            };
        },

        buildTemplate: function (userinfo) {
            return [
					'<section id="LockScreenInline" class="body-sign body-locked body-locked-inline">',
						'<div class="center-sign">',
							'<div class="panel panel-sign">',
								'<div class="panel-body">',
									'<form>',
										'<div class="current-user text-center">',
											'<img id="LockUserPicture" src="{{picture}}" alt="John Doe" class="img-circle user-image" />',
											'<h2 id="LockUserName" class="user-name text-dark m-none">{{username}}</h2>',
											'<p  id="LockUserEmail" class="user-email m-none">{{email}}</p>',
										'</div>',
										'<div class="form-group mb-lg">',
											'<div class="input-group input-group-icon">',
												'<input id="pwd" name="pwd" type="password" class="form-control input-lg" placeholder="Password" />',
												'<span class="input-group-addon">',
													'<span class="icon icon-lg">',
														'<i class="fa fa-lock"></i>',
													'</span>',
												'</span>',
											'</div>',
										'</div>',

										'<div class="row">',
											'<div class="col-xs-6">',
												'<p class="mt-xs mb-none">',
													'<a href="#">Not John Doe?</a>',
												'</p>',
											'</div>',
											'<div class="col-xs-6 text-right">',
												'<button type="submit" class="btn btn-primary">Unlock</button>',
											'</div>',
										'</div>',
									'</form>',
								'</div>',
							'</div>',
						'</div>',
					'</section>'
            ]
				.join('')
				.replace(/\{\{picture\}\}/, userinfo.picture)
				.replace(/\{\{username\}\}/, userinfo.username)
				.replace(/\{\{email\}\}/, userinfo.email);
        }

    };

    this.LockScreen = LockScreen;

    $(function () {
        LockScreen.initialize();
    });

}).apply(this, [jQuery]);

// Panels
(function ($) {

    $(function () {
        $('.panel')
			.on('click', '.panel-actions a.fa-caret-up', function (e) {
			    e.preventDefault();

			    var $this,
					$panel;

			    $this = $(this);
			    $panel = $this.closest('.panel');

			    $this
					.removeClass('fa-caret-up')
					.addClass('fa-caret-down');

			    $panel.find('.panel-body, .panel-footer').slideDown(200);
			})
			.on('click', '.panel-actions a.fa-caret-down', function (e) {
			    e.preventDefault();

			    var $this,
					$panel;

			    $this = $(this);
			    $panel = $this.closest('.panel');

			    $this
					.removeClass('fa-caret-down')
					.addClass('fa-caret-up');

			    $panel.find('.panel-body, .panel-footer').slideUp(200);
			})
			.on('click', '.panel-actions a.closebtn', function (e) {
			    e.preventDefault();

			    var $panel,
					$row;

			    $panel = $(this).closest('.panel');

			    if (!!($panel.parent('div').attr('class') || '').match(/col-(xs|sm|md|lg)/g) && $panel.siblings().length === 0) {
			        $row = $panel.closest('.row');
			        $panel.parent('div').remove();
			        if ($row.children().length === 0) {
			            $row.remove();
			        }
			    } else {
			        $panel.remove();
			    }
			});
    });

})(jQuery);

// Bootstrap Toggle
(function ($) {

    'use strict';

    var $window = $(window);

    var toggleClass = function ($el) {
        if (!!$el.data('toggleClassBinded')) {
            return false;
        }

        var $target,
			className,
			eventName;

        $target = $($el.attr('data-target'));
        className = $el.attr('data-toggle-class');
        eventName = $el.attr('data-fire-event');


        $el.on('click.toggleClass', function (e) {
            e.preventDefault();
            $target.toggleClass(className);

            var hasClass = $target.hasClass(className);

            if (!!eventName) {
                $window.trigger(eventName, {
                    added: hasClass,
                    removed: !hasClass
                });
            }
        });

        $el.data('toggleClassBinded', true);

        return true;
    };

    $(function () {
        $('[data-toggle-class][data-target]').each(function () {
            toggleClass($(this));
        });
    });

}).apply(this, [jQuery]);

// Form to Object
(function ($) {

    'use strict';

    $.fn.formToObject = function () {
        var arrayData,
			objectData;

        arrayData = this.serializeArray();
        objectData = {};

        $.each(arrayData, function () {
            var value;

            if (this.value != null) {
                value = this.value;
            } else {
                value = '';
            }

            if (objectData[this.name] != null) {
                if (!objectData[this.name].push) {
                    objectData[this.name] = [objectData[this.name]];
                }

                objectData[this.name].push(value);
            } else {
                objectData[this.name] = value;
            }
        });

        return objectData;
    };

})(jQuery);


// Colorpicker
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__colorpicker';

    var PluginColorPicker = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginColorPicker.defaults = {
    };

    PluginColorPicker.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginColorPicker.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.colorpicker(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginColorPicker: PluginColorPicker
    });

    // jquery plugin
    $.fn.themePluginColorPicker = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginColorPicker($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Datepicker
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__datepicker';

    var PluginDatePicker = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginDatePicker.defaults = {
    };

    PluginDatePicker.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setVars()
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setVars: function () {
            this.skin = this.$el.data('plugin-skin');

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginDatePicker.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.datepicker(this.options);

            if (!!this.skin) {
                this.$el.data('datepicker').picker.addClass('datepicker-' + this.skin);
            }

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginDatePicker: PluginDatePicker
    });

    // jquery plugin
    $.fn.themePluginDatePicker = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginDatePicker($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// iosSwitcher
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__IOS7Switch';

    var PluginIOS7Switch = function ($el) {
        return this.initialize($el);
    };

    PluginIOS7Switch.prototype = {
        initialize: function ($el) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        build: function () {
            var switcher = new Switch(this.$el.get(0));

            $(switcher.el).on('click', function (e) {
                e.preventDefault();
                switcher.toggle();
            });

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginIOS7Switch: PluginIOS7Switch
    });

    // jquery plugin
    $.fn.themePluginIOS7Switch = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginIOS7Switch($this);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);



// Masked Input
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__maskedInput';

    var PluginMaskedInput = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginMaskedInput.defaults = {
    };

    PluginMaskedInput.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginMaskedInput.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.mask(this.$el.data('input-mask'), this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginMaskedInput: PluginMaskedInput
    });

    // jquery plugin
    $.fn.themePluginMaskedInput = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginMaskedInput($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// MaxLength
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__maxlength';

    var PluginMaxLength = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginMaxLength.defaults = {
        alwaysShow: true,
        placement: 'bottom-left',
        warningClass: 'label label-success bottom-left',
        limitReachedClass: 'label label-danger bottom-left'
    };

    PluginMaxLength.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginMaxLength.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.maxlength(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginMaxLength: PluginMaxLength
    });

    // jquery plugin
    $.fn.themePluginMaxLength = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginMaxLength($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// MultiSelect
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__multiselect';

    var PluginMultiSelect = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginMultiSelect.defaults = {
        templates: {
            filter: '<div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div>'
        }
    };

    PluginMultiSelect.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginMultiSelect.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.multiselect(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginMultiSelect: PluginMultiSelect
    });

    // jquery plugin
    $.fn.themePluginMultiSelect = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginMultiSelect($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Select2
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__select2';

    var PluginSelect2 = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginSelect2.defaults = {
    };

    PluginSelect2.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginSelect2.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.select2(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginSelect2: PluginSelect2
    });

    // jquery plugin
    $.fn.themePluginSelect2 = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginSelect2($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Spinner
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__spinner';

    var PluginSpinner = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginSpinner.defaults = {
    };

    PluginSpinner.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginSpinner.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.spinner(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginSpinner: PluginSpinner
    });

    // jquery plugin
    $.fn.themePluginSpinner = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginSpinner($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);


// TextArea AutoSize
(function (theme, $) {

    theme = theme || {};

    var initialized = false;
    var instanceName = '__textareaAutosize';

    var PluginTextAreaAutoSize = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginTextAreaAutoSize.defaults = {
    };

    PluginTextAreaAutoSize.prototype = {
        initialize: function ($el, opts) {
            if (initialized) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginTextAreaAutoSize.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.autosize(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginTextAreaAutoSize: PluginTextAreaAutoSize
    });

    // jquery plugin
    $.fn.themePluginTextAreaAutoSize = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginTextAreaAutoSize($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// TimePicker
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__timepicker';

    var PluginTimePicker = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginTimePicker.defaults = {
        disableMousewheel: true
    };

    PluginTimePicker.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginTimePicker.defaults, opts);

            return this;
        },

        build: function () {
            this.$el.timepicker(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginTimePicker: PluginTimePicker
    });

    // jquery plugin
    $.fn.themePluginTimePicker = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginTimePicker($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);


// Animate
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__animate';

    var PluginAnimate = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginAnimate.defaults = {
        accX: 0,
        accY: -150,
        delay: 1
    };

    PluginAnimate.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginAnimate.defaults, opts, {
                wrapper: this.$el
            });

            return this;
        },

        build: function () {
            var self = this,
				$el = this.options.wrapper,
				delay = 0;

            $el.addClass('appear-animation');

            if (!$('html').hasClass('no-csstransitions') && $(window).width() > 767) {

                $el.appear(function () {

                    delay = ($el.attr('data-appear-animation-delay') ? $el.attr('data-appear-animation-delay') : self.options.delay);

                    if (delay > 1) {
                        $el.css('animation-delay', delay + 'ms');
                    }

                    $el.addClass($el.attr('data-appear-animation'));

                    setTimeout(function () {
                        $el.addClass('appear-animation-visible');
                    }, delay);

                }, { accX: self.options.accX, accY: self.options.accY });

            } else {

                $el.addClass('appear-animation-visible');

            }

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginAnimate: PluginAnimate
    });

    // jquery plugin
    $.fn.themePluginAnimate = function (opts) {
        return this.map(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginAnimate($this, opts);
            }

        });
    };

}).apply(this, [window.theme, jQuery]);

// Carousel
(function (theme, $) {

    theme = theme || {};

    var initialized = false;
    var instanceName = '__carousel';

    var PluginCarousel = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginCarousel.defaults = {
        itemsDesktop: false,
        itemsDesktopSmall: false,
        itemsTablet: false,
        itemsTabletSmall: false,
        itemsMobile: false
    };

    PluginCarousel.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginCarousel.defaults, opts, {
                wrapper: this.$el
            });

            return this;
        },

        build: function () {
            this.options.wrapper.owlCarousel(this.options).addClass("owl-carousel-init");

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginCarousel: PluginCarousel
    });

    // jquery plugin
    $.fn.themePluginCarousel = function (opts) {
        return this.map(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginCarousel($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Chart Circular
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__chartCircular';


    var PluginChartCircular = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginChartCircular.defaults = {
        accX: 0,
        accY: -150,
        delay: 1,
        barColor: '#0088CC',
        trackColor: '#f2f2f2',
        scaleColor: false,
        scaleLength: 5,
        lineCap: 'round',
        lineWidth: 13,
        size: 175,
        rotate: 0,
        animate: ({
            duration: 2500,
            enabled: true
        })
    };

    PluginChartCircular.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginChartCircular.defaults, opts, {
                wrapper: this.$el
            });

            return this;
        },

        build: function () {
            var self = this,
				$el = this.options.wrapper,
				value = ($el.attr('data-percent') ? $el.attr('data-percent') : 0),
				percentEl = $el.find('.percent'),
				shouldAnimate,
				data;

            shouldAnimate = $.isFunction($.fn['appear']) && (typeof $.browser !== 'undefined' && !$.browser.mobile);
            data = { accX: self.options.accX, accY: self.options.accY };

            $.extend(true, self.options, {
                onStep: function (from, to, currentValue) {
                    percentEl.html(parseInt(currentValue));
                }
            });

            $el.attr('data-percent', (shouldAnimate ? 0 : value));

            $el.easyPieChart(this.options);

            if (shouldAnimate) {
                $el.appear(function () {
                    setTimeout(function () {
                        $el.data('easyPieChart').update(value);
                        $el.attr('data-percent', value);

                    }, self.options.delay);
                }, data);
            } else {
                $el.data('easyPieChart').update(value);
                $el.attr('data-percent', value);
            }

            return this;
        }
    };

    // expose to scope
    $.extend(true, theme, {
        Chart: {
            PluginChartCircular: PluginChartCircular
        }
    });

    // jquery plugin
    $.fn.themePluginChartCircular = function (opts) {
        return this.map(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginChartCircular($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Lightbox
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__lightbox';

    var PluginLightbox = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginLightbox.defaults = {
        tClose: 'Close (Esc)', // Alt text on close button
        tLoading: 'Loading...', // Text that is displayed during loading. Can contain %curr% and %total% keys
        gallery: {
            tPrev: 'Previous (Left arrow key)', // Alt text on left arrow
            tNext: 'Next (Right arrow key)', // Alt text on right arrow
            tCounter: '%curr% of %total%' // Markup for "1 of 7" counter
        },
        image: {
            tError: '<a href="%url%">The image</a> could not be loaded.' // Error message when image could not be loaded
        },
        ajax: {
            tError: '<a href="%url%">The content</a> could not be loaded.' // Error message when ajax request failed
        }
    };

    PluginLightbox.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginLightbox.defaults, opts, {
                wrapper: this.$el
            });

            return this;
        },

        build: function () {
            this.options.wrapper.magnificPopup(this.options);

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginLightbox: PluginLightbox
    });

    // jquery plugin
    $.fn.themePluginLightbox = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginLightbox($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Slider
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__slider';

    var PluginSlider = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginSlider.defaults = {

    };

    PluginSlider.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setVars()
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setVars: function () {
            var $output = $(this.$el.data('plugin-slider-output'));
            this.$output = $output.get(0) ? $output : null;

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            var _self = this;
            this.options = $.extend(true, {}, PluginSlider.defaults, opts);

            if (this.$output) {
                $.extend(this.options, {
                    slide: function (event, ui) {
                        _self.onSlide(event, ui);
                    }
                });
            }

            return this;
        },

        build: function () {
            this.$el.slider(this.options);

            return this;
        },

        onSlide: function (event, ui) {
            if (!ui.values) {
                this.$output.val(ui.value);
            } else {
                this.$output.val(ui.values[0] + '/' + ui.values[1]);
            }

            this.$output.trigger('change');
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginSlider: PluginSlider
    });

    // jquery plugin
    $.fn.themePluginSlider = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginSlider($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Toggle
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__toggle';

    var PluginToggle = function ($el, opts) {
        return this.initialize($el, opts);
    };

    PluginToggle.defaults = {
        duration: 350,
        isAccordion: false,
        addIcons: true
    };

    PluginToggle.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, PluginToggle.defaults, opts, {
                wrapper: this.$el
            });

            return this;
        },

        build: function () {
            var self = this,
				$wrapper = this.options.wrapper,
				$items = $wrapper.find('.toggle'),
				$el = null;

            $items.each(function () {
                $el = $(this);

                if (self.options.addIcons) {
                    $el.find('> label').prepend(
						$('<i />').addClass('fa fa-plus'),
						$('<i />').addClass('fa fa-minus')
					);
                }

                if ($el.hasClass('active')) {
                    $el.find('> p').addClass('preview-active');
                    $el.find('> .toggle-content').slideDown(self.options.duration);
                }

                self.events($el);
            });

            if (self.options.isAccordion) {
                self.options.duration = self.options.duration / 2;
            }

            return this;
        },

        events: function ($el) {
            var self = this,
				previewParCurrentHeight = 0,
				previewParAnimateHeight = 0,
				toggleContent = null;

            $el.find('> label').click(function (e) {

                var $this = $(this),
					parentSection = $this.parent(),
					parentWrapper = $this.parents('.toggle'),
					previewPar = null,
					closeElement = null;

                if (self.options.isAccordion && typeof (e.originalEvent) != 'undefined') {
                    closeElement = parentWrapper.find('.toggle.active > label');

                    if (closeElement[0] == $this[0]) {
                        return;
                    }
                }

                parentSection.toggleClass('active');

                // Preview Paragraph
                if (parentSection.find('> p').get(0)) {

                    previewPar = parentSection.find('> p');
                    previewParCurrentHeight = previewPar.css('height');
                    previewPar.css('height', 'auto');
                    previewParAnimateHeight = previewPar.css('height');
                    previewPar.css('height', previewParCurrentHeight);

                }

                // Content
                toggleContent = parentSection.find('> .toggle-content');

                if (parentSection.hasClass('active')) {

                    $(previewPar).animate({
                        height: previewParAnimateHeight
                    }, self.options.duration, function () {
                        $(this).addClass('preview-active');
                    });

                    toggleContent.slideDown(self.options.duration, function () {
                        if (closeElement) {
                            closeElement.trigger('click');
                        }
                    });

                } else {

                    $(previewPar).animate({
                        height: 0
                    }, self.options.duration, function () {
                        $(this).removeClass('preview-active');
                    });

                    toggleContent.slideUp(self.options.duration);

                }

            });
        }
    };

    // expose to scope
    $.extend(theme, {
        PluginToggle: PluginToggle
    });

    // jquery plugin
    $.fn.themePluginToggle = function (opts) {
        return this.map(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new PluginToggle($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Widget - Todo
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__widgetTodoList';

    var WidgetTodoList = function ($el, opts) {
        return this.initialize($el, opts);
    };

    WidgetTodoList.defaults = {
    };

    WidgetTodoList.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build()
				.events();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, WidgetTodoList.defaults, opts);

            return this;
        },

        check: function (input, label) {
            if (input.is(':checked')) {
                label.addClass('line-through');
            } else {
                label.removeClass('line-through');
            }
        },

        build: function () {
            var _self = this,
				$check = this.$el.find('.todo-check');

            $check.each(function () {
                var label = $(this).closest('li').find('.todo-label');
                _self.check($(this), label);
            });

            return this;
        },

        events: function () {
            var _self = this,
				$remove = this.$el.find('.todo-remove'),
				$check = this.$el.find('.todo-check'),
				$window = $(window);

            $remove.on('click.widget-todo-list', function (ev) {
                ev.preventDefault();
                $(this).closest("li").remove();
            });

            $check.on('change', function () {
                var label = $(this).closest('li').find('.todo-label');
                _self.check($(this), label);
            });

            if ($.isFunction($.fn.sortable)) {
                this.$el.sortable({
                    sort: function (event, ui) {
                        var top = event.pageY - _self.$el.offset().top - (ui.helper.outerHeight(true) / 2);
                        ui.helper.css({ 'top': top + 'px' });
                    }
                });
            }

            return this;
        }
    };

    // expose to scope
    $.extend(theme, {
        WidgetTodoList: WidgetTodoList
    });

    // jquery plugin
    $.fn.themePluginWidgetTodoList = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new WidgetTodoList($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Widget - Toggle
(function (theme, $) {

    theme = theme || {};

    var instanceName = '__widgetToggleExpand';

    var WidgetToggleExpand = function ($el, opts) {
        return this.initialize($el, opts);
    };

    WidgetToggleExpand.defaults = {
    };

    WidgetToggleExpand.prototype = {
        initialize: function ($el, opts) {
            if ($el.data(instanceName)) {
                return this;
            }

            this.$el = $el;

            this
				.setData()
				.setOptions(opts)
				.build()
				.events();

            return this;
        },

        setData: function () {
            this.$el.data(instanceName, this);

            return this;
        },

        setOptions: function (opts) {
            this.options = $.extend(true, {}, WidgetToggleExpand.defaults, opts);

            return this;
        },

        build: function () {
            return this;
        },

        events: function () {
            var _self = this,
				$toggler = this.$el.find('.widget-toggle');

            $toggler.on('click.widget-toggler', function () {
                _self.$el.hasClass('widget-collapsed') ? _self.expand(_self.$el) : _self.collapse(_self.$el);
            });

            return this;
        },

        expand: function (content) {
            content.children('.widget-content-expanded').slideDown('fast', function () {
                $(this).css('display', '');
                content.removeClass('widget-collapsed');
            });
        },

        collapse: function (content) {
            content.children('.widget-content-expanded').slideUp('fast', function () {
                content.addClass('widget-collapsed');
                $(this).css('display', '');
            });
        }
    };

    // expose to scope
    $.extend(theme, {
        WidgetToggleExpand: WidgetToggleExpand
    });

    // jquery plugin
    $.fn.themePluginWidgetToggleExpand = function (opts) {
        return this.each(function () {
            var $this = $(this);

            if ($this.data(instanceName)) {
                return $this.data(instanceName);
            } else {
                return new WidgetToggleExpand($this, opts);
            }

        });
    }

}).apply(this, [window.theme, jQuery]);

// Data Tables - Config
(function ($) {

    'use strict';

    // we overwrite initialize of all datatables here
    // because we want to use select2, give search input a bootstrap look
    // keep in mind if you overwrite this fnInitComplete somewhere,
    // you should run the code inside this function to keep functionality.
    //
    // there's no better way to do this at this time :(
    if ($.isFunction($.fn['dataTable'])) {

        $.extend(true, $.fn.dataTable.defaults, {
            sDom: "<'row datatables-header form-inline'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>r><'table-responsive Of-a't><'row datatables-footer m-none mt-sm'<'col-sm-12 col-md-6 p-none'i><'col-sm-12 col-md-6 p-none'p>>",
            oLanguage: {
                sLengthMenu: '_MENU_ records per page',
                sProcessing: '<i class="fa fa-spinner fa-spin"></i> Loading'
            },
            fnInitComplete: function (settings, json) {
                // select 2
                if ($.isFunction($.fn['select2'])) {
                    $('.dataTables_length select', settings.nTableWrapper).select2({
                        minimumResultsForSearch: -1
                    });
                }

                var options = $('table', settings.nTableWrapper).data('plugin-options') || {};

                // search
                var $search = $('.dataTables_filter input', settings.nTableWrapper);

                $search
					.attr({
					    placeholder: typeof options.searchPlaceholder !== 'undefined' ? options.searchPlaceholder : 'Search'
					})
					.addClass('form-control');

                if ($.isFunction($.fn.placeholder)) {
                    $search.placeholder();
                }
            }
        });

    }

}).apply(this, [jQuery]);

// Notifications - Config
(function ($) {

    'use strict';

    // use font awesome icons if available
    if (typeof PNotify != 'undefined') {
        PNotify.prototype.options.styling = "fontawesome";

        $.extend(true, PNotify.prototype.options, {
            shadow: false,
            stack: {
                spacing1: 15,
                spacing2: 15
            }
        });

        $.extend(PNotify.styling.fontawesome, {
            // classes
            container: "notification",
            notice: "notification-warning",
            info: "notification-info",
            success: "notification-success",
            error: "notification-danger",

            // icons
            notice_icon: "fa fa-exclamation",
            info_icon: "fa fa-info",
            success_icon: "fa fa-check",
            error_icon: "fa fa-times"
        });
    }

}).apply(this, [jQuery]);